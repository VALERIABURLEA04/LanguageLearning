using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Factories;
using sa.Models;
using sa.Services;

namespace Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly TokenService _tokens;
    private readonly PostLoginStrategyFactory _postLogin;
    public HomeController(ApplicationDbContext db, TokenService tokens, PostLoginStrategyFactory postLogin)
    {
        _db = db;
        _tokens = tokens;
        _postLogin = postLogin;
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

    public IActionResult Courses() => View(_db.Courses.OrderBy(c => c.Id).ToList());

    public IActionResult CourseDetail(int id)
    {
        var course = _db.Courses.Find(id);
        if (course == null) return NotFound();
        ViewBag.Lessons = _db.Lessons.Where(l => l.CourseTitle == course.Title).ToList();
        return View(course);
    }

    public IActionResult Enroll(int id)
    {
        var course = _db.Courses.Find(id);
        if (course == null) return NotFound();
        course.Students += 1;
        _db.SaveChanges();
        TempData["Msg"] = $"You're enrolled in \"{course.Title}\". Start with Lesson 1 below.";
        return RedirectToAction(nameof(CourseDetail), new { id });
    }

    public IActionResult Contact() => View();

    // ========== AUTH ==========

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == email);
        if (user == null || !PasswordHasher.Verify(password ?? "", user.PasswordHash))
        {
            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        await SignInUserAsync(user);
        TempData["AuthToken"] = _tokens.Generate(user);

        var outcome = _postLogin.Create(user.IsAdmin).Execute(user.Name);
        TempData["Msg"] = outcome.WelcomeMessage;
        return RedirectToAction(outcome.RedirectAction, outcome.RedirectController);
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "All fields are required.";
            return View();
        }
        if (_db.Users.Any(u => u.Email == email))
        {
            ViewBag.Error = "An account with this email already exists.";
            return View();
        }

        var user = new User
        {
            Name         = name.Trim(),
            Email        = email.Trim(),
            PasswordHash = PasswordHasher.Hash(password),
            IsAdmin      = false
        };
        _db.Users.Add(user);
        _db.SaveChanges();

        await SignInUserAsync(user);
        TempData["AuthToken"] = _tokens.Generate(user);

        var outcome = _postLogin.Create(user.IsAdmin).Execute(user.Name);
        TempData["Msg"] = outcome.WelcomeMessage;
        return RedirectToAction(outcome.RedirectAction, outcome.RedirectController);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["ClearToken"] = true;
        return RedirectToAction(nameof(Index));
    }

    private async Task SignInUserAsync(User user)
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

        var user = _db.Users.Find(id);
        if (user == null) return RedirectToAction(nameof(Login));

        var purchases = _db.Purchases
            .Where(p => p.StudentName == user.Name)
            .OrderByDescending(p => p.PurchaseDate)
            .ToList();
        var enrolledTitles = purchases.Select(p => p.CourseTitle).Distinct().ToList();
        var enrolled = _db.Courses.Where(c => enrolledTitles.Contains(c.Title)).ToList();

        ViewBag.User = user;
        ViewBag.Purchases = purchases;
        ViewBag.EnrolledCourses = enrolled;
        return View();
    }
}
