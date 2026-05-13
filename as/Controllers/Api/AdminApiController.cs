using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Facade;

namespace Controllers.Api;

[ApiController]
[Route("api/admin")]
[Produces("application/json")]
public class AdminApiController : ControllerBase
{
    private readonly AdminFacade _admin;
    public AdminApiController(AdminFacade admin) => _admin = admin;

    // ---------- DASHBOARD ----------

    /// <summary>Aggregate counts: courses / lessons / students / revenue.</summary>
    [HttpGet("stats")]
    public IActionResult Stats() => Ok(_admin.GetStats());

    /// <summary>Undo the most recent admin write (CommandInvoker history).</summary>
    [HttpPost("undo")]
    public IActionResult Undo()
    {
        _admin.UndoLast();
        return Ok(new { success = true, message = "Last admin action undone." });
    }

    // ---------- COURSES ----------

    [HttpGet("courses")]
    public IActionResult ListCourses() => Ok(_admin.ListCourses());

    [HttpGet("courses/{id:int}")]
    public IActionResult GetCourse(int id)
    {
        var c = _admin.FindCourse(id);
        return c is null ? NotFound() : Ok(c);
    }

    [HttpPost("courses")]
    public IActionResult CreateCourse([FromBody] CourseDto input) =>
        Ok(_admin.CreateCourse(input));

    [HttpPut("courses/{id:int}")]
    public IActionResult UpdateCourse(int id, [FromBody] CourseDto input)
    {
        var dto = input with { Id = id };
        return _admin.UpdateCourse(dto)
            ? Ok(new { success = true })
            : NotFound(new { error = $"Course {id} not found." });
    }

    [HttpDelete("courses/{id:int}")]
    public IActionResult DeleteCourse(int id) =>
        _admin.DeleteCourse(id)
            ? Ok(new { success = true })
            : NotFound(new { error = $"Course {id} not found." });

    // ---------- LESSONS ----------

    [HttpGet("lessons")]
    public IActionResult ListLessons() => Ok(_admin.ListLessons());

    [HttpGet("lessons/{id:int}")]
    public IActionResult GetLesson(int id)
    {
        var l = _admin.FindLesson(id);
        return l is null ? NotFound() : Ok(l);
    }

    [HttpPost("lessons")]
    public IActionResult CreateLesson([FromBody] LessonDto input) =>
        Ok(_admin.CreateLesson(input));

    [HttpPut("lessons/{id:int}")]
    public IActionResult UpdateLesson(int id, [FromBody] LessonDto input)
    {
        var dto = input with { Id = id };
        return _admin.UpdateLesson(dto)
            ? Ok(new { success = true })
            : NotFound(new { error = $"Lesson {id} not found." });
    }

    [HttpDelete("lessons/{id:int}")]
    public IActionResult DeleteLesson(int id) =>
        _admin.DeleteLesson(id)
            ? Ok(new { success = true })
            : NotFound(new { error = $"Lesson {id} not found." });

    // ---------- PURCHASES ----------

    [HttpGet("purchases")]
    public IActionResult ListPurchases() => Ok(_admin.ListPurchases());

    [HttpGet("purchases/{id:int}")]
    public IActionResult GetPurchase(int id)
    {
        var p = _admin.FindPurchase(id);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpPost("purchases")]
    public IActionResult CreatePurchase([FromBody] PurchaseDto input) =>
        Ok(_admin.CreatePurchase(input));

    [HttpPut("purchases/{id:int}")]
    public IActionResult UpdatePurchase(int id, [FromBody] PurchaseDto input)
    {
        var dto = input with { Id = id };
        return _admin.UpdatePurchase(dto)
            ? Ok(new { success = true })
            : NotFound(new { error = $"Purchase {id} not found." });
    }

    [HttpDelete("purchases/{id:int}")]
    public IActionResult DeletePurchase(int id) =>
        _admin.DeletePurchase(id)
            ? Ok(new { success = true })
            : NotFound(new { error = $"Purchase {id} not found." });

    // ---------- USERS ----------

    /// <summary>List all users with their IsAdmin flag.</summary>
    [HttpGet("users")]
    public IActionResult ListUsers() =>
        Ok(_admin.ListUsers().Select(u => new
        {
            u.Id, u.Name, u.Email, u.IsAdmin, u.CreatedAt
        }));

    [HttpGet("users/{id:int}")]
    public IActionResult GetUser(int id)
    {
        var u = _admin.FindUser(id);
        return u is null
            ? NotFound(new { error = $"User {id} not found." })
            : Ok(new { u.Id, u.Name, u.Email, u.IsAdmin, u.CreatedAt });
    }

    public class SetAdminInput { public bool IsAdmin { get; set; } }

    /// <summary>Promote / demote a user. Body: { "isAdmin": true }. Reversible via /api/admin/undo.</summary>
    [HttpPatch("users/{id:int}/admin")]
    public IActionResult SetUserAdmin(int id, [FromBody] SetAdminInput input) =>
        _admin.SetUserAdmin(id, input.IsAdmin)
            ? Ok(new { success = true, userId = id, isAdmin = input.IsAdmin })
            : NotFound(new { error = $"User {id} not found." });

    [HttpDelete("users/{id:int}")]
    public IActionResult DeleteUser(int id) =>
        _admin.DeleteUser(id)
            ? Ok(new { success = true })
            : NotFound(new { error = $"User {id} not found." });
}
