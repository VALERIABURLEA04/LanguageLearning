using sa.Swagger;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyWebApplication.BusinessLogic.Command;
using MyWebApplication.BusinessLogic.Core;
using MyWebApplication.BusinessLogic.Factories;
using MyWebApplication.BusinessLogic.Facade;
using MyWebApplication.BusinessLogic.Interfaces;
using MyWebApplication.BusinessLogic.Payments;
using sa.Models;
using sa.Services;
using sa.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Lingua API", Version = "v1" });
    c.SchemaFilter<DefaultValueSchemaFilter>();
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// Stores (EF-backed implementations of BusinessLogic interfaces)
builder.Services.AddScoped<IUserStore,     EfUserStore>();
builder.Services.AddScoped<ICourseStore,   EfCourseStore>();
builder.Services.AddScoped<ILessonStore,   EfLessonStore>();
builder.Services.AddScoped<IPurchaseStore, EfPurchaseStore>();

// Cross-cutting services
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<ITokenIssuer,    TokenService>();
builder.Services.AddSingleton<PostLoginStrategyFactory>();

// Business-logic services (Facade / Command / Core)
builder.Services.AddScoped<AuthFacade>();
builder.Services.AddScoped<EnrollStudentCommand>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<CourseQueryService>();
builder.Services.AddScoped<CommandInvoker>();
builder.Services.AddScoped<AdminFacade>();

// Payments (Adapter + Strategy/Factory)
builder.Services.AddSingleton<StripeAPI>();
builder.Services.AddSingleton<PayPalAPI>();
builder.Services.AddSingleton<StripeAdapter>();
builder.Services.AddSingleton<PayPalAdapter>();
builder.Services.AddSingleton<PaymentStrategyFactory>();
builder.Services.AddScoped<CheckoutCommand>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db     = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    db.Database.EnsureCreated();

    // Add new columns to Courses if the DB already exists without them
    var courseColumns = new[]
    {
        ("OldPrice",        "REAL NOT NULL DEFAULT 0"),
        ("Description",     "TEXT NOT NULL DEFAULT ''"),
        ("LongDescription", "TEXT NOT NULL DEFAULT ''"),
        ("ImageUrl",        "TEXT NOT NULL DEFAULT ''"),
        ("BackgroundColor", "TEXT NOT NULL DEFAULT '#FF6B35'"),
        ("Icon",            "TEXT NOT NULL DEFAULT '📚'"),
        ("Duration",        "TEXT NOT NULL DEFAULT ''"),
        ("Frequency",       "TEXT NOT NULL DEFAULT ''"),
        ("PriceNote",       "TEXT NOT NULL DEFAULT ''"),
        ("FeaturesText",    "TEXT NOT NULL DEFAULT ''"),
    };
    foreach (var (col, type) in courseColumns)
    {
        try { db.Database.ExecuteSqlRaw($"ALTER TABLE Courses ADD COLUMN {col} {type}"); }
        catch { /* column already exists — ignore */ }
    }

    if (!db.Users.Any())
    {
        var adminEmail = app.Configuration["Seed:AdminEmail"] ?? "admin@lingua.app";
        var adminPwd   = app.Configuration["Seed:AdminPassword"]
                         ?? throw new InvalidOperationException("Seed:AdminPassword is not configured in appsettings.json.");
        db.Users.Add(new User
        {
            Name = "Site Admin", Email = adminEmail,
            PasswordHash = hasher.Hash(adminPwd), IsAdmin = true
        });
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lingua API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllers(); // attribute-routed API endpoints

app.Run();
