using Microsoft.EntityFrameworkCore;
using sa.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<CourseRow> Courses   { get; set; } = null!;
    public DbSet<LessonRow> Lessons   { get; set; } = null!;
    public DbSet<Purchase>  Purchases { get; set; } = null!;
    public DbSet<User>      Users     { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>().HasIndex(u => u.Email).IsUnique();

        b.Entity<CourseRow>().HasData(
            new CourseRow { Id = 1, Title = "Adult English Beginner", Language = "English", Level = "A1", Price = 99,  Students = 142, Lessons = 24 },
            new CourseRow { Id = 2, Title = "Business English",       Language = "English", Level = "B2", Price = 199, Students = 78,  Lessons = 36 },
            new CourseRow { Id = 3, Title = "Cambridge Prep",         Language = "English", Level = "C1", Price = 249, Students = 53,  Lessons = 48 },
            new CourseRow { Id = 4, Title = "Conversation Club",      Language = "English", Level = "B1", Price = 49,  Students = 210, Lessons = 12 },
            new CourseRow { Id = 5, Title = "Teen English",           Language = "English", Level = "A2", Price = 89,  Students = 96,  Lessons = 20 }
        );

        b.Entity<LessonRow>().HasData(
            new LessonRow { Id = 1, Title = "Greetings & Introductions", CourseTitle = "Adult English Beginner", DurationMinutes = 45, Status = "Published" },
            new LessonRow { Id = 2, Title = "Daily Routines",            CourseTitle = "Adult English Beginner", DurationMinutes = 50, Status = "Published" },
            new LessonRow { Id = 3, Title = "Business Emails",           CourseTitle = "Business English",       DurationMinutes = 60, Status = "Published" },
            new LessonRow { Id = 4, Title = "Negotiation Skills",        CourseTitle = "Business English",       DurationMinutes = 75, Status = "Draft" },
            new LessonRow { Id = 5, Title = "Reading FCE Tasks",         CourseTitle = "Cambridge Prep",         DurationMinutes = 90, Status = "Published" },
            new LessonRow { Id = 6, Title = "Free Talk Friday",          CourseTitle = "Conversation Club",      DurationMinutes = 40, Status = "Published" },
            new LessonRow { Id = 7, Title = "Slang for Teens",           CourseTitle = "Teen English",           DurationMinutes = 35, Status = "Draft" }
        );

        var baseDate = new DateTime(2026, 5, 12, 12, 0, 0, DateTimeKind.Utc);
        b.Entity<Purchase>().HasData(
            new Purchase { Id = 1, StudentName = "Maria Popescu",  CourseTitle = "Adult English Beginner", Amount = 99,  PurchaseDate = baseDate.AddDays(-1),  Status = "Completed", PaymentMethod = "Stripe" },
            new Purchase { Id = 2, StudentName = "Andrei Ionescu", CourseTitle = "Business English",       Amount = 199, PurchaseDate = baseDate.AddDays(-2),  Status = "Completed", PaymentMethod = "PayPal" },
            new Purchase { Id = 3, StudentName = "Elena Ciobanu",  CourseTitle = "Cambridge Prep",         Amount = 249, PurchaseDate = baseDate.AddDays(-3),  Status = "Pending",   PaymentMethod = "Stripe" },
            new Purchase { Id = 4, StudentName = "Vlad Marinescu", CourseTitle = "Conversation Club",      Amount = 49,  PurchaseDate = baseDate.AddDays(-5),  Status = "Completed", PaymentMethod = "Card" },
            new Purchase { Id = 5, StudentName = "Ana Dumitrescu", CourseTitle = "Teen English",           Amount = 89,  PurchaseDate = baseDate.AddDays(-7),  Status = "Refunded",  PaymentMethod = "PayPal" },
            new Purchase { Id = 6, StudentName = "Cristian Bălan", CourseTitle = "Business English",       Amount = 199, PurchaseDate = baseDate.AddDays(-10), Status = "Completed", PaymentMethod = "Stripe" },
            new Purchase { Id = 7, StudentName = "Maria Popescu",  CourseTitle = "Adult English Beginner", Amount = 99,  PurchaseDate = baseDate.AddDays(-12), Status = "Completed", PaymentMethod = "Card" }
        );
    }
}
