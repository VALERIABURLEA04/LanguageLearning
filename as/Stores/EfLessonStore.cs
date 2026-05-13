using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;
using sa.Models;

namespace sa.Stores;

public class EfLessonStore : ILessonStore
{
    private readonly ApplicationDbContext _db;
    public EfLessonStore(ApplicationDbContext db) => _db = db;

    public LessonDto? FindById(int id) =>
        _db.Lessons.Find(id) is { } l ? Map(l) : null;

    public IReadOnlyList<LessonDto> ListAll() =>
        _db.Lessons.OrderBy(l => l.Id).Select(l => Map(l)).ToList();

    public IReadOnlyList<LessonDto> ListByCourseTitle(string courseTitle) =>
        _db.Lessons.Where(l => l.CourseTitle == courseTitle).Select(l => Map(l)).ToList();

    public LessonDto Create(LessonDto lesson)
    {
        var row = new LessonRow
        {
            Title           = lesson.Title,
            CourseTitle     = lesson.CourseTitle,
            DurationMinutes = lesson.DurationMinutes,
            Status          = lesson.Status
        };
        _db.Lessons.Add(row);
        _db.SaveChanges();
        return Map(row);
    }

    public bool Update(LessonDto lesson)
    {
        var row = _db.Lessons.Find(lesson.Id);
        if (row == null) return false;
        row.Title           = lesson.Title;
        row.CourseTitle     = lesson.CourseTitle;
        row.DurationMinutes = lesson.DurationMinutes;
        row.Status          = lesson.Status;
        _db.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var row = _db.Lessons.Find(id);
        if (row == null) return false;
        _db.Lessons.Remove(row);
        _db.SaveChanges();
        return true;
    }

    private static LessonDto Map(LessonRow l) =>
        new(l.Id, l.Title, l.CourseTitle, l.DurationMinutes, l.Status);
}
