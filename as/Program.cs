using sa.Swagger;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyWebApplication.BusinessLogic.Command;
using MyWebApplication.BusinessLogic.Core;
using MyWebApplication.BusinessLogic.Factories;
using MyWebApplication.BusinessLogic.Facade;
using MyWebApplication.BusinessLogic.Interfaces;
using MyWebApplication.BusinessLogic.Observer;
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
builder.Services.AddScoped<IUserStore,         EfUserStore>();
builder.Services.AddScoped<ICourseStore,       EfCourseStore>();
builder.Services.AddScoped<ILessonStore,       EfLessonStore>();
builder.Services.AddScoped<IPurchaseStore,     EfPurchaseStore>();
builder.Services.AddScoped<INotificationStore, EfNotificationStore>();

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
builder.Services.AddScoped<NotificationObserver>();
builder.Services.AddScoped<CourseNotifier>();
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

    db.Database.ExecuteSqlRaw("""
        CREATE TABLE IF NOT EXISTS Notifications (
            Id        INTEGER PRIMARY KEY AUTOINCREMENT,
            Message   TEXT    NOT NULL DEFAULT '',
            IsRead    INTEGER NOT NULL DEFAULT 0,
            CreatedAt TEXT    NOT NULL DEFAULT ''
        )
    """);

    {
        var adminEmail = app.Configuration["Seed:AdminEmail"] ?? "admin@lingua.app";
        var adminPwd   = app.Configuration["Seed:AdminPassword"]
                         ?? throw new InvalidOperationException("Seed:AdminPassword is not configured in appsettings.json.");
        var adminUser  = db.Users.FirstOrDefault(u => u.Email == adminEmail);
        if (adminUser == null)
        {
            db.Users.Add(new User
            {
                Name = "Site Admin", Email = adminEmail,
                PasswordHash = hasher.Hash(adminPwd), IsAdmin = true
            });
        }
        else
        {
            // always re-sync hash so appsettings.json is the source of truth
            adminUser.PasswordHash = hasher.Hash(adminPwd);
            adminUser.IsAdmin = true;
        }
        db.SaveChanges();
    }

    // Seed default courses (skip if title already exists)
    var seedCourses = new[]
    {
        new CourseRow {
            Title = "Engleză pentru adulți",
            Language = "English", Level = "A1 — B2",
            Price = 2990, OldPrice = 3990,
            Description = "Curs complet pentru adulți care vor să vorbească engleză cu încredere.",
            LongDescription = "Programul nostru pentru adulți îmbină gramatică practică, vocabular esențial și conversații reale conduse de profesori certificați CELTA/DELTA. Grupuri mici (max 6 persoane) garantează atenție individuală la fiecare lecție.",
            BackgroundColor = "#FF6B35", Icon = "📚",
            Duration = "12 săptămâni", Frequency = "2 lecții/săptămână",
            PriceNote = "Disponibil și în 3 rate fără dobândă",
            FeaturesText = "Grupuri mici (max 6 cursanți)\nProfesori certificați CELTA/DELTA\nMateriale Cambridge incluse\nFeedback 1:1 săptămânal\nCertificat de absolvire"
        },
        new CourseRow {
            Title = "Engleză pentru copii",
            Language = "English", Level = "A1 — A2",
            Price = 1990, OldPrice = 0,
            Description = "Lecții interactive și jucăușe pentru copii cu vârste între 6–12 ani.",
            LongDescription = "Copiii învață engleză prin jocuri, cântece și activități creative adaptate vârstei. Profesorii noștri specializați în educație timpurie creează un mediu sigur și distractiv în care copilul capătă drag de limbă.",
            BackgroundColor = "#0CA678", Icon = "🎮",
            Duration = "10 săptămâni", Frequency = "2 lecții/săptămână",
            PriceNote = "Vârstă recomandată: 6–12 ani",
            FeaturesText = "Metode interactive și jocuri educative\nGrupe de maxim 5 copii\nProfesori specializați în lucrul cu copiii\nTeme scurte și distractive\nRaport lunar pentru părinți"
        },
        new CourseRow {
            Title = "Engleză pentru adolescenți",
            Language = "English", Level = "A2 — B1",
            Price = 2490, OldPrice = 0,
            Description = "Program adaptat pentru tineri 13–17 ani — vorbire, scriere și cultură.",
            LongDescription = "Adolescenții explorează engleza prin teme relevante lor: muzică, film, social media, sport și cultură. Curriculum-ul B1 îi pregătește și pentru cerințele școlare și examenele internaționale.",
            BackgroundColor = "#7B2FBE", Icon = "🎓",
            Duration = "12 săptămâni", Frequency = "2 lecții/săptămână",
            PriceNote = "Potrivit și ca pregătire suplimentară la școală",
            FeaturesText = "Teme actuale și relevante pentru tineri\nDezbatere și prezentări în engleză\nGrupe de vârstă omogenă\nPregătire pentru cerințe școlare\nActivități online interactive"
        },
        new CourseRow {
            Title = "Lecții individuale de engleză",
            Language = "English", Level = "Orice nivel",
            Price = 450, OldPrice = 0,
            Description = "Sesiuni 1:1 personalizate după obiectivele, nivelul și programul tău.",
            LongDescription = "Cel mai rapid mod de a avansa: tutorele se adaptează 100% nevoilor tale. Alegi obiectivul (conversație, business, examen) și programul. Nici un minut pierdut pe ce știi deja.",
            BackgroundColor = "#00B4D8", Icon = "👤",
            Duration = "Flexibil", Frequency = "La alegere",
            PriceNote = "Preț per lecție de 60 min · Pachete disponibile",
            FeaturesText = "Program 100% flexibil\nCurriculum personalizat pe obiectivele tale\nFeedback imediat și detaliat\nMateriale adaptate nivelului\nPosibilitate de sesiuni online sau fizic"
        },
        new CourseRow {
            Title = "Pregătire pentru BAC",
            Language = "English", Level = "B1 — B2",
            Price = 2990, OldPrice = 0,
            Description = "Intensiv de engleză orientat 100% spre structura și cerințele examenului BAC.",
            LongDescription = "Parcurgem sistematic toate subiectele din programa BAC: reading comprehension, essay writing, grammar și listening. Exerciții din examene anterioare, simulări complete și strategii de punctaj maxim.",
            BackgroundColor = "#FFB800", Icon = "📝",
            Duration = "16 săptămâni", Frequency = "3 lecții/săptămână",
            PriceNote = "Recomandat din clasa a 10-a",
            FeaturesText = "Structură aliniată 100% la programa BAC\nSimulări de examen periodice\nEseuri corectate cu feedback detaliat\nStrategii pentru timp și punctaj\nSuport și în afara orelor"
        },
        new CourseRow {
            Title = "Pregătire TOEFL",
            Language = "English", Level = "B2 — C1",
            Price = 3990, OldPrice = 4990,
            Description = "Pregătire completă pentru TOEFL iBT — Reading, Listening, Speaking, Writing.",
            LongDescription = "Cursul acoperă toate cele 4 secțiuni TOEFL iBT cu strategii specifice fiecăreia. Practică pe teste reale, înregistrări Speaking evaluate, eseuri Writing cu feedback și simulări timed la fiecare 2 săptămâni.",
            BackgroundColor = "#1B3A6B", Icon = "🏆",
            Duration = "14 săptămâni", Frequency = "3 lecții/săptămână",
            PriceNote = "Include acces la bancă de teste originale",
            FeaturesText = "Toate cele 4 secțiuni TOEFL iBT\nStrategii dovedite pentru scor 90+\nSimulări complete timed\nSpeaking evaluat cu înregistrări\nEseuri Writing corectate individual"
        },
    };

    foreach (var course in seedCourses)
    {
        if (!db.Courses.Any(c => c.Title == course.Title))
            db.Courses.Add(course);
    }
    db.SaveChanges();

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
app.UseStaticFiles();
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
