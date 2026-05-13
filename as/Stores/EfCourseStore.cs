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
        _db.Courses.OrderBy(c => c.Id).Select(c => Map(c)).ToList();

    public IReadOnlyList<CourseDto> ListByTitles(IReadOnlyList<string> titles) =>
        _db.Courses.Where(c => titles.Contains(c.Title)).Select(c => Map(c)).ToList();

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
        r.Title           = d.Title;
        r.Language        = d.Language;
        r.Level           = d.Level;
        r.Price           = d.Price;
        r.OldPrice        = d.OldPrice;
        r.Students        = d.Students;
        r.Lessons         = d.Lessons;
        r.Description     = d.Description;
        r.LongDescription = d.LongDescription;
        r.ImageUrl        = d.ImageUrl;
        r.BackgroundColor = d.BackgroundColor;
        r.Icon            = d.Icon;
        r.Duration        = d.Duration;
        r.Frequency       = d.Frequency;
        r.PriceNote       = d.PriceNote;
        r.FeaturesText    = d.FeaturesText;
        return r;
    }

    private static CourseDto Map(CourseRow c) =>
        new(c.Id, c.Title, c.Language, c.Level, c.Price, c.Students, c.Lessons)
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
