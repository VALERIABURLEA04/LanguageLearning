using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Command;
using MyWebApplication.BusinessLogic.Core;
using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Facade;
using MyWebApplication.BusinessLogic.Interfaces;

namespace Controllers;

public class HomeController : Controller
{
    private readonly AuthFacade           _auth;
    private readonly EnrollStudentCommand _enroll;
    private readonly ProfileService       _profile;
    private readonly CourseQueryService   _courseQuery;
    private readonly CheckoutCommand      _checkout;
    private readonly INotificationStore   _notifications;

    public HomeController(
        AuthFacade auth,
        EnrollStudentCommand enroll,
        ProfileService profile,
        CourseQueryService courseQuery,
        CheckoutCommand checkout,
        INotificationStore notifications)
    {
        _auth          = auth;
        _enroll        = enroll;
        _profile       = profile;
        _courseQuery   = courseQuery;
        _checkout      = checkout;
        _notifications = notifications;
    }

    public IActionResult Index()
    {
        var stats = _courseQuery.GetPublicStats();
        ViewBag.Courses       = _courseQuery.ListAll().Select(d => new sa.Models.CourseRow
        {
            Id = d.Id, Title = d.Title, Language = d.Language, Level = d.Level,
            Price = d.Price, OldPrice = d.OldPrice, Students = d.Students, Lessons = d.Lessons,
            Description = d.Description, ImageUrl = d.ImageUrl,
            BackgroundColor = d.BackgroundColor, Icon = d.Icon
        }).ToList();
        ViewBag.TotalCourses  = stats.TotalCourses;
        ViewBag.TotalLessons  = stats.TotalLessons;
        ViewBag.TotalStudents = stats.TotalStudents;
        return View();
    }

    public IActionResult Privacy() => View();

    public IActionResult Courses(string? q)
    {
        var results = _courseQuery.Search(q).Select(d => new sa.Models.CourseRow
        {
            Id = d.Id, Title = d.Title, Language = d.Language, Level = d.Level,
            Price = d.Price, OldPrice = d.OldPrice, Students = d.Students, Lessons = d.Lessons,
            Description = d.Description, ImageUrl = d.ImageUrl,
            BackgroundColor = d.BackgroundColor, Icon = d.Icon
        }).ToList();
        ViewBag.Query = q ?? string.Empty;
        return View(results);
    }

    public IActionResult CourseDetail(int id)
    {
        var bundle = _courseQuery.GetWithLessons(id);
        if (bundle is null) return NotFound();

        var c = bundle.Value.Course;
        var course = new sa.Models.CourseRow
        {
            Id = c.Id, Title = c.Title, Language = c.Language, Level = c.Level,
            Price = c.Price, OldPrice = c.OldPrice, Students = c.Students, Lessons = c.Lessons,
            Description = c.Description, LongDescription = c.LongDescription,
            ImageUrl = c.ImageUrl, BackgroundColor = c.BackgroundColor,
            Icon = c.Icon, Duration = c.Duration, Frequency = c.Frequency,
            PriceNote = c.PriceNote, FeaturesText = c.FeaturesText
        };
        ViewBag.Lessons = bundle.Value.Lessons
            .Select(l => new sa.Models.LessonRow { Id = l.Id, Title = l.Title, CourseTitle = l.CourseTitle, DurationMinutes = l.DurationMinutes, Status = l.Status })
            .ToList();

        var userId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var uid) ? uid : 0;
        var profileBundle = userId > 0 ? _profile.GetForUser(userId) : null;
        ViewBag.IsEnrolled = profileBundle?.EnrolledCourses.Any(ec => ec.Id == id) ?? false;

        return View(course);
    }

    [Authorize]
    public IActionResult Notifications()
    {
        _notifications.MarkAllRead();
        return View(_notifications.ListAll());
    }

    public IActionResult Enroll(int id)
    {
        var result = _enroll.Execute(id);
        if (!result.Success) return NotFound();

        TempData["Msg"] = $"You're enrolled in \"{result.CourseTitle}\". Start with Lesson 1 below.";
        return RedirectToAction(nameof(CourseDetail), new { id });
    }

    public IActionResult Contact() => View();

    public IActionResult LevelTest() => View();

    public IActionResult Category(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return NotFound();
        var category = sa.Models.CourseCategory.FindBySlug(id);
        if (category is null) return NotFound();
        return View(category);
    }

    [HttpGet]
    public IActionResult Checkout(int id)
    {
        var dto = _courseQuery.FindById(id);
        if (dto is null) return NotFound();
        return View(new sa.Models.CourseRow
        {
            Id = dto.Id, Title = dto.Title, Language = dto.Language, Level = dto.Level,
            Price = dto.Price, OldPrice = dto.OldPrice, Students = dto.Students, Lessons = dto.Lessons,
            Description = dto.Description, LongDescription = dto.LongDescription,
            ImageUrl = dto.ImageUrl, BackgroundColor = dto.BackgroundColor,
            Icon = dto.Icon, Duration = dto.Duration, Frequency = dto.Frequency,
            PriceNote = dto.PriceNote, FeaturesText = dto.FeaturesText
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Checkout(int id, string fullName, string email, string phone, string paymentMethod, string plan)
    {
        var course = _courseQuery.FindById(id);
        if (course is null) return NotFound();

        _checkout.Setup(new CheckoutRequest
        {
            CategorySlug  = course.Id.ToString(),
            CourseTitle   = course.Title,
            FullName      = fullName,
            Email         = email,
            Phone         = phone,
            PaymentMethod = paymentMethod,
            Plan          = plan,
            TotalAmount   = course.Price
        });
        _checkout.Execute();
        var result = _checkout.Result;

        if (!result.Success)
        {
            TempData["Error"] = result.Error;
            return RedirectToAction(nameof(Checkout), new { id });
        }

        TempData["Msg"] = result.Message;
        return RedirectToAction(nameof(CourseDetail), new { id });
    }

    // ========== AUTH ==========

    [HttpGet] public IActionResult Login() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = _auth.Login(email, password);
        return await CompleteAuthAsync(result);
    }

    [HttpGet] public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(string name, string email, string password)
    {
        var result = _auth.Register(name, email, password);
        return await CompleteAuthAsync(result);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["ClearToken"] = true;
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> CompleteAuthAsync(AuthResult result)
    {
        if (!result.Success || result.User is null || result.Outcome is null)
        {
            ViewBag.Error = result.Error;
            return View();
        }

        await SignInUserAsync(result.User);
        TempData["AuthToken"] = result.Token;
        TempData["Msg"] = result.Outcome.WelcomeMessage;
        return RedirectToAction(result.Outcome.RedirectAction, result.Outcome.RedirectController);
    }

    private async Task SignInUserAsync(UserDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name,           user.Name),
            new(ClaimTypes.Email,          user.Email),
            new(ClaimTypes.Role,           user.IsAdmin ? "Admin" : "User")
        };
        var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }

    // ========== PROFILE ==========

    [Authorize]
    public IActionResult Profile()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(idStr, out var id)) return RedirectToAction(nameof(Login));

        var bundle = _profile.GetForUser(id);
        if (bundle is null) return RedirectToAction(nameof(Login));

        ViewBag.User = new sa.Models.User
        {
            Id = bundle.User.Id, Name = bundle.User.Name, Email = bundle.User.Email,
            PasswordHash = bundle.User.PasswordHash, IsAdmin = bundle.User.IsAdmin, CreatedAt = bundle.User.CreatedAt
        };
        ViewBag.Purchases = bundle.Purchases
            .Select(p => new sa.Models.Purchase { Id = p.Id, StudentName = p.StudentName, CourseTitle = p.CourseTitle, Amount = p.Amount, PurchaseDate = p.PurchaseDate, Status = p.Status, PaymentMethod = p.PaymentMethod })
            .ToList();
        ViewBag.EnrolledCourses = bundle.EnrolledCourses
            .Select(c => new sa.Models.CourseRow { Id = c.Id, Title = c.Title, Language = c.Language, Level = c.Level, Price = c.Price, Students = c.Students, Lessons = c.Lessons })
            .ToList();
        return View();
    }
}
