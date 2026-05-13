using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Command;
using MyWebApplication.BusinessLogic.Core;

namespace Controllers.Api;

[ApiController]
[Route("api/courses")]
[Produces("application/json")]
public class CoursesApiController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly CourseQueryService _courseQuery;
    private readonly EnrollStudentCommand _enroll;

    public CoursesApiController(
        ApplicationDbContext db,
        CourseQueryService courseQuery,
        EnrollStudentCommand enroll)
    {
        _db = db;
        _courseQuery = courseQuery;
        _enroll = enroll;
    }

    /// <summary>List all courses from the database.</summary>
    [HttpGet]
    public IActionResult List() => Ok(_db.Courses.OrderBy(c => c.Id).ToList());

    /// <summary>Get one course with its lessons.</summary>
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var bundle = _courseQuery.GetWithLessons(id);
        if (bundle is null) return NotFound(new { error = $"Course {id} not found." });

        return Ok(new
        {
            course  = bundle.Value.Course,
            lessons = bundle.Value.Lessons
        });
    }

    /// <summary>Enroll a student in a course (Command pattern).</summary>
    [HttpPost("{id:int}/enroll")]
    public IActionResult Enroll(int id)
    {
        var result = _enroll.Execute(id);
        if (!result.Success) return NotFound(new { error = result.Error });

        return Ok(new
        {
            success     = true,
            courseTitle = result.CourseTitle,
            message     = $"Enrolled in \"{result.CourseTitle}\"."
        });
    }
}
