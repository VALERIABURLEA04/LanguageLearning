using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;
using sa.Models;

namespace sa.Stores;

public class EfCourseStore : ICourseStore
{
    private readonly ApplicationDbContext _db;
    public EfCourseStore(ApplicationDbContext db) => _db = db;

    public CourseDto? FindById(int id) =>
        _db.Courses.Find(id) is { } c ? Map(c) : null;

    public IReadOnlyList<CourseDto> ListAll() =>
        _db.Courses.OrderBy(c => c.Id).ToList().Select(Map).ToList();

    public IReadOnlyList<CourseDto> ListByTitles(IReadOnlyList<string> titles) =>
        _db.Courses.Where(c => titles.Contains(c.Title)).ToList().Select(Map).ToList();

    public bool IncrementStudents(int courseId)
    {
        var course = _db.Courses.Find(courseId);
        if (course == null) return false;
        course.Students += 1;
        _db.SaveChanges();
        return true;
    }

    public CourseDto Create(CourseDto dto)
    {
        var row = ToRow(dto);
        _db.Courses.Add(row);
        _db.SaveChanges();
        return Map(row);
    }

    public bool Update(CourseDto dto)
    {
        var row = _db.Courses.Find(dto.Id);
        if (row == null) return false;
        ApplyDto(dto, row);
        _db.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var row = _db.Courses.Find(id);
        if (row == null) return false;
        _db.Courses.Remove(row);
        _db.SaveChanges();
        return true;
    }

    private static CourseRow ToRow(CourseDto d) => ApplyDto(d, new CourseRow());

    private static CourseRow ApplyDto(CourseDto d, CourseRow r)
    {
        r.Title           = d.Title           ?? string.Empty;
        r.Language        = d.Language        ?? string.Empty;
        r.Level           = d.Level           ?? string.Empty;
        r.Price           = d.Price;
        r.OldPrice        = d.OldPrice;
        r.Students        = d.Students;
        r.Description     = d.Description     ?? string.Empty;
        r.LongDescription = d.LongDescription ?? string.Empty;
        r.ImageUrl        = d.ImageUrl        ?? string.Empty;
        r.BackgroundColor = string.IsNullOrEmpty(d.BackgroundColor) ? "#FF6B35" : d.BackgroundColor;
        r.Icon            = string.IsNullOrEmpty(d.Icon)            ? "📚"     : d.Icon;
        r.Duration        = d.Duration        ?? string.Empty;
        r.Frequency       = d.Frequency       ?? string.Empty;
        r.PriceNote       = d.PriceNote       ?? string.Empty;
        r.FeaturesText    = d.FeaturesText    ?? string.Empty;
        return r;
    }

    private CourseDto Map(CourseRow c)
    {
        var lessonCount = _db.Lessons.Count(l => l.CourseTitle == c.Title);
        return new(c.Id, c.Title, c.Language, c.Level, c.Price, c.Students, lessonCount)
        {
            OldPrice        = c.OldPrice,
            Description     = c.Description,
            LongDescription = c.LongDescription,
            ImageUrl        = c.ImageUrl,
            BackgroundColor = c.BackgroundColor,
            Icon            = c.Icon,
            Duration        = c.Duration,
            Frequency       = c.Frequency,
            PriceNote       = c.PriceNote,
            FeaturesText    = c.FeaturesText
        };
    }
}
