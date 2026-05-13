using MyWebApplication.BusinessLogic.Command;
using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;
using MyWebApplication.BusinessLogic.Observer;

namespace MyWebApplication.BusinessLogic.Facade;

/// <summary>
/// Facade for admin operations: groups Course / Lesson / Purchase CRUD.
/// Uses Command pattern (per-operation) + CommandInvoker (for undo history) +
/// Iterator pattern when listing course catalogs.
/// </summary>
public class AdminFacade
{
    private readonly ICourseStore   _courses;
    private readonly ILessonStore   _lessons;
    private readonly IPurchaseStore _purchases;
    private readonly IUserStore     _users;
    private readonly CommandInvoker _invoker;
    private readonly CourseNotifier _notifier;

    public AdminFacade(
        ICourseStore courses,
        ILessonStore lessons,
        IPurchaseStore purchases,
        IUserStore users,
        CommandInvoker invoker,
        CourseNotifier notifier)
    {
        _courses   = courses;
        _lessons   = lessons;
        _purchases = purchases;
        _users     = users;
        _invoker   = invoker;
        _notifier  = notifier;
    }

    // ---------- READ ----------
    public IReadOnlyList<CourseDto>   ListCourses()   => _courses.ListAll();
    public IReadOnlyList<LessonDto>   ListLessons()   => _lessons.ListAll();
    public IReadOnlyList<PurchaseDto> ListPurchases() => _purchases.ListAll();
    public CourseDto?   FindCourse(int id)   => _courses.FindById(id);
    public LessonDto?   FindLesson(int id)   => _lessons.FindById(id);
    public PurchaseDto? FindPurchase(int id) => _purchases.FindById(id);

    public AdminStats GetStats()
    {
        var courses = _courses.ListAll();
        var purchases = _purchases.ListAll();
        return new AdminStats(
            TotalCourses:  courses.Count,
            TotalLessons:  _lessons.ListAll().Count,
            TotalStudents: courses.Sum(c => c.Students),
            TotalRevenue:  purchases.Where(p => p.Status == "Completed").Sum(p => p.Amount)
        );
    }

    // ---------- WRITE — each operation goes through CommandInvoker so it lives in history ----------

    public CourseDto CreateCourse(CourseDto input)
    {
        var cmd = new CreateCourseCommand(_courses, input);
        _invoker.ExecuteCommand(cmd);
        _notifier.CourseAdded(input.Title);
        return cmd.Created!;
    }
    public bool UpdateCourse(CourseDto input)
    {
        var cmd = new UpdateCourseCommand(_courses, input);
        _invoker.ExecuteCommand(cmd);
        return cmd.Success;
    }
    public bool DeleteCourse(int id)
    {
        var cmd = new DeleteCourseCommand(_courses, id);
        _invoker.ExecuteCommand(cmd);
        return cmd.Success;
    }

    public LessonDto CreateLesson(LessonDto input)
    {
        var cmd = new CreateLessonCmd(_lessons, input);
        _invoker.ExecuteCommand(cmd);
        _notifier.LessonAdded(input.CourseTitle, input.Title);
        return cmd.Created!;
    }
    public bool UpdateLesson(LessonDto input)
    {
        var cmd = new UpdateLessonCmd(_lessons, input);
        _invoker.ExecuteCommand(cmd);
        return cmd.Success;
    }
    public bool DeleteLesson(int id)
    {
        var cmd = new DeleteLessonCmd(_lessons, id);
        _invoker.ExecuteCommand(cmd);
        return cmd.Success;
    }

    public PurchaseDto CreatePurchase(PurchaseDto input)
    {
        var cmd = new CreatePurchaseCmd(_purchases, input);
        _invoker.ExecuteCommand(cmd);
        return cmd.Created!;
    }
    public bool UpdatePurchase(PurchaseDto input)
    {
        var cmd = new UpdatePurchaseCmd(_purchases, input);
        _invoker.ExecuteCommand(cmd);
        return cmd.Success;
    }
    public bool DeletePurchase(int id)
    {
        var cmd = new DeletePurchaseCmd(_purchases, id);
        _invoker.ExecuteCommand(cmd);
        return cmd.Success;
    }

    // ---------- USERS ----------

    public IReadOnlyList<UserDto> ListUsers() => _users.ListAll();
    public UserDto? FindUser(int id) => _users.FindById(id);

    public bool SetUserAdmin(int userId, bool isAdmin)
    {
        var cmd = new SetUserAdminCommand(_users, userId, isAdmin);
        _invoker.ExecuteCommand(cmd);
        return cmd.Success;
    }

    public bool DeleteUser(int userId)
    {
        var cmd = new DeleteUserCommand(_users, userId);
        _invoker.ExecuteCommand(cmd);
        return cmd.Success;
    }

    /// <summary>Rolls back the most recent admin write (CommandInvoker.Undo).</summary>
    public void UndoLast() => _invoker.Undo();
}

public record AdminStats(int TotalCourses, int TotalLessons, int TotalStudents, decimal TotalRevenue);
