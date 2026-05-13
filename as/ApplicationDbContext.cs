using Microsoft.EntityFrameworkCore;
using sa.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<CourseRow>   Courses       { get; set; } = null!;
    public DbSet<LessonRow>   Lessons       { get; set; } = null!;
    public DbSet<Purchase>    Purchases     { get; set; } = null!;
    public DbSet<User>        Users         { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>().HasIndex(u => u.Email).IsUnique();
    }
}
