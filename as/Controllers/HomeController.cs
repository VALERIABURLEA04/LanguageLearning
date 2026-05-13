using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Command;
using MyWebApplication.BusinessLogic.Core;
using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Facade;

namespace Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly AuthFacade _auth;
    private readonly EnrollStudentCommand _enroll;
    private readonly ProfileService _profile;
    private readonly CourseQueryService _courseQuery;
    private readonly CheckoutCommand _checkout;

    public HomeController(
        ApplicationDbContext db,
        AuthFacade auth,
        EnrollStudentCommand enroll,
        ProfileService profile,
        CourseQueryService courseQuery,
        CheckoutCommand checkout)
    {
        _db = db;
        _auth = auth;
        _enroll = enroll;
        _profile = profile;
        _courseQuery = courseQuery;
        _checkout = checkout;
    }

    public IActionResult Index()
    {
        ViewBag.Courses       = _db.Courses.OrderBy(c => c.Id).ToList();
        ViewBag.TotalCourses  = _db.Courses.Count();
        ViewBag.TotalLessons  = _db.Lessons.Count();
        ViewBag.TotalStudents = _db.Courses.Sum(c => c.Students);
        return View();
    }

    public IActionResult Privacy() => View();

    public IActionResult Courses(string? q)
    {
        var query = _db.Courses.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(c => c.Title.Contains(q) || c.Description.Contains(q) || c.Level.Contains(q));
        ViewBag.Query = q ?? string.Empty;
        return View(query.OrderBy(c => c.Id).ToList());
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
        return View(course);
    }

    public IActionResult Enroll(int id)
    {
        var result = _enroll.Execute(id);
        if (!result.Success) return NotFound();

        TempData["Msg"] = $"You're enrolled in \"{result.CourseTitle}\". Start with Lesson 1 below.";
        return RedirectToAction(nameof(CourseDetail), new { id });
    }

    public IActionResult Contact() => View();

    public IActionResult Category(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return NotFound();
        var category = sa.Models.CourseCategory.FindBySlug(id);
        if (category is null) return NotFound();
        return View(category);
    }

    [HttpGet]
    public IActionResult Checkout(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return NotFound();
        var category = sa.Models.CourseCategory.FindBySlug(id);
        if (category is null) return NotFound();
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Checkout(string id, string fullName, string email, string phone, string paymentMethod, string plan)
    {
        var category = sa.Models.CourseCategory.FindBySlug(id);
        if (category is null) return NotFound();

        var result = _checkout.Execute(new CheckoutRequest
        {
            CategorySlug   = category.Slug,
            CourseTitle    = category.Title,
            FullName       = fullName,
            Email          = email,
            Phone          = phone,
            PaymentMethod  = paymentMethod,
            Plan           = plan,
            TotalAmount    = category.Price
        });

        if (!result.Success)
        {
            TempData["Error"] = result.Error;
            return RedirectToAction(nameof(Checkout), new { id });
        }

        TempData["Msg"] = result.Message;
        return RedirectToAction(nameof(Category), new { id });
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
