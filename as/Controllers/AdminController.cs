using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Facade;
using sa.Models;

namespace Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AdminFacade _admin;
    private readonly IWebHostEnvironment _env;

    public AdminController(AdminFacade admin, IWebHostEnvironment env)
    {
        _admin = admin;
        _env   = env;
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Msg"] = "You have been logged out.";
        TempData["ClearToken"] = true;
        return RedirectToAction("Login", "Home");
    }

    public IActionResult Index()
    {
        var stats = _admin.GetStats();
        ViewBag.TotalCourses    = stats.TotalCourses;
        ViewBag.TotalLessons    = stats.TotalLessons;
        ViewBag.TotalStudents   = stats.TotalStudents;
        ViewBag.TotalRevenue    = stats.TotalRevenue;
        ViewBag.RecentPurchases = _admin.ListPurchases()
            .OrderByDescending(p => p.PurchaseDate).Take(5)
            .Select(PurchaseToRow).ToList();
        return View();
    }

    // ============================== COURSES ==============================

    public IActionResult Courses() =>
        View(_admin.ListCourses().Select(CourseToRow).ToList());

    [HttpGet]
    public IActionResult CreateCourse()
    {
        ViewBag.FormTitle = "Adaugă curs";
        return View("CourseForm", new CourseRow { Level = "A1", Language = "English", BackgroundColor = "#FF6B35", Icon = "📚" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCourse(CourseRow model, IFormFile? imageFile)
    {
        if (!ModelState.IsValid) { ViewBag.FormTitle = "Adaugă curs"; return View("CourseForm", model); }
        model.ImageUrl = await SaveImageAsync(imageFile) ?? string.Empty;
        _admin.CreateCourse(CourseToDto(model));
        TempData["Msg"] = $"Cursul \"{model.Title}\" a fost creat.";
        return RedirectToAction(nameof(Courses));
    }

    [HttpGet]
    public IActionResult EditCourse(int id)
    {
        var course = _admin.FindCourse(id);
        if (course is null) return NotFound();
        ViewBag.FormTitle = "Editează curs";
        return View("CourseForm", CourseToRow(course));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCourse(CourseRow model, IFormFile? imageFile, string? existingImageUrl)
    {
        var existing = _admin.FindCourse(model.Id);
        if (existing is null) return NotFound();
        var newImage = await SaveImageAsync(imageFile);
        model.ImageUrl = newImage ?? existingImageUrl ?? existing.ImageUrl;
        _admin.UpdateCourse(CourseToDto(model));
        TempData["Msg"] = $"Cursul \"{model.Title}\" a fost actualizat.";
        return RedirectToAction(nameof(Courses));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteCourse(int id)
    {
        var course = _admin.FindCourse(id);
        if (course is not null)
        {
            _admin.DeleteCourse(id);
            TempData["Msg"] = $"Cursul \"{course.Title}\" a fost șters.";
        }
        return RedirectToAction(nameof(Courses));
    }

    // ============================== LESSONS ==============================

    public IActionResult Lessons() =>
        View(_admin.ListLessons().Select(LessonToRow).ToList());

    [HttpGet]
    public IActionResult CreateLesson()
    {
        ViewBag.FormTitle    = "Add Lesson";
        ViewBag.CourseTitles = _admin.ListCourses().Select(c => c.Title).ToList();
        return View("LessonForm", new LessonRow { Status = "Published", DurationMinutes = 45 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateLesson(LessonRow model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.FormTitle    = "Add Lesson";
            ViewBag.CourseTitles = _admin.ListCourses().Select(c => c.Title).ToList();
            return View("LessonForm", model);
        }
        _admin.CreateLesson(LessonToDto(model));
        TempData["Msg"] = $"Lesson \"{model.Title}\" created.";
        return RedirectToAction(nameof(Lessons));
    }

    [HttpGet]
    public IActionResult EditLesson(int id)
    {
        var lesson = _admin.FindLesson(id);
        if (lesson is null) return NotFound();
        ViewBag.FormTitle    = "Edit Lesson";
        ViewBag.CourseTitles = _admin.ListCourses().Select(c => c.Title).ToList();
        return View("LessonForm", LessonToRow(lesson));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditLesson(LessonRow model)
    {
        if (_admin.FindLesson(model.Id) is null) return NotFound();
        _admin.UpdateLesson(LessonToDto(model));
        TempData["Msg"] = $"Lesson \"{model.Title}\" updated.";
        return RedirectToAction(nameof(Lessons));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteLesson(int id)
    {
        var lesson = _admin.FindLesson(id);
        if (lesson is not null)
        {
            _admin.DeleteLesson(id);
            TempData["Msg"] = $"Lesson \"{lesson.Title}\" deleted.";
        }
        return RedirectToAction(nameof(Lessons));
    }

    // ============================== PURCHASES ==============================

    public IActionResult Purchases() =>
        View(_admin.ListPurchases().OrderByDescending(p => p.PurchaseDate).Select(PurchaseToRow).ToList());

    [HttpGet]
    public IActionResult CreatePurchase()
    {
        ViewBag.FormTitle    = "Add Purchase";
        ViewBag.CourseTitles = _admin.ListCourses().Select(c => c.Title).ToList();
        return View("PurchaseForm", new Purchase { PurchaseDate = DateTime.Now, Status = "Completed", PaymentMethod = "Stripe" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreatePurchase(Purchase model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.FormTitle    = "Add Purchase";
            ViewBag.CourseTitles = _admin.ListCourses().Select(c => c.Title).ToList();
            return View("PurchaseForm", model);
        }
        _admin.CreatePurchase(PurchaseToDto(model));
        TempData["Msg"] = $"Purchase created.";
        return RedirectToAction(nameof(Purchases));
    }

    [HttpGet]
    public IActionResult EditPurchase(int id)
    {
        var purchase = _admin.FindPurchase(id);
        if (purchase is null) return NotFound();
        ViewBag.FormTitle    = "Edit Purchase";
        ViewBag.CourseTitles = _admin.ListCourses().Select(c => c.Title).ToList();
        return View("PurchaseForm", PurchaseToRow(purchase));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditPurchase(Purchase model)
    {
        if (_admin.FindPurchase(model.Id) is null) return NotFound();
        _admin.UpdatePurchase(PurchaseToDto(model));
        TempData["Msg"] = $"Purchase #{model.Id} updated.";
        return RedirectToAction(nameof(Purchases));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePurchase(int id)
    {
        var purchase = _admin.FindPurchase(id);
        if (purchase is not null)
        {
            _admin.DeletePurchase(id);
            TempData["Msg"] = $"Purchase #{id} deleted.";
        }
        return RedirectToAction(nameof(Purchases));
    }

    // ============================== USERS ==============================

    public IActionResult Users() =>
        View(_admin.ListUsers().Select(u => new User
        {
            Id = u.Id, Name = u.Name, Email = u.Email,
            PasswordHash = u.PasswordHash, IsAdmin = u.IsAdmin, CreatedAt = u.CreatedAt
        }).ToList());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleAdmin(int id)
    {
        var user = _admin.FindUser(id);
        if (user is not null)
        {
            var newState = !user.IsAdmin;
            _admin.SetUserAdmin(id, newState);
            TempData["Msg"] = $"{user.Name} este acum {(newState ? "Admin" : "User")}.";
        }
        return RedirectToAction(nameof(Users));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteUser(int id)
    {
        var user = _admin.FindUser(id);
        if (user is not null)
        {
            _admin.DeleteUser(id);
            TempData["Msg"] = $"Userul \"{user.Name}\" a fost șters.";
        }
        return RedirectToAction(nameof(Users));
    }

    // ============================== HELPERS ==============================

    private async Task<string?> SaveImageAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0) return null;
        var dir = Path.Combine(_env.WebRootPath, "uploads", "courses");
        Directory.CreateDirectory(dir);
        var ext  = Path.GetExtension(file.FileName).ToLowerInvariant();
        var name = $"{Guid.NewGuid():N}{ext}";
        await using var stream = System.IO.File.Create(Path.Combine(dir, name));
        await file.CopyToAsync(stream);
        return $"/uploads/courses/{name}";
    }

    // ============================== MAPPING ==============================

    private static CourseRow CourseToRow(CourseDto d) => new()
    {
        Id = d.Id, Title = d.Title, Language = d.Language, Level = d.Level,
        Price = d.Price, OldPrice = d.OldPrice, Students = d.Students, Lessons = d.Lessons,
        Description = d.Description, LongDescription = d.LongDescription,
        ImageUrl = d.ImageUrl, BackgroundColor = d.BackgroundColor,
        Icon = d.Icon, Duration = d.Duration, Frequency = d.Frequency,
        PriceNote = d.PriceNote, FeaturesText = d.FeaturesText
    };

    private static CourseDto CourseToDto(CourseRow m) =>
        new(m.Id, m.Title, m.Language, m.Level, m.Price, m.Students, m.Lessons)
        {
            OldPrice = m.OldPrice, Description = m.Description, LongDescription = m.LongDescription,
            ImageUrl = m.ImageUrl, BackgroundColor = m.BackgroundColor, Icon = m.Icon,
            Duration = m.Duration, Frequency = m.Frequency, PriceNote = m.PriceNote, FeaturesText = m.FeaturesText
        };

    private static LessonRow LessonToRow(LessonDto d) => new()
        { Id = d.Id, Title = d.Title, CourseTitle = d.CourseTitle, DurationMinutes = d.DurationMinutes, Status = d.Status };

    private static LessonDto LessonToDto(LessonRow m) =>
        new(m.Id, m.Title, m.CourseTitle, m.DurationMinutes, m.Status);

    private static Purchase PurchaseToRow(PurchaseDto d) => new()
        { Id = d.Id, StudentName = d.StudentName, CourseTitle = d.CourseTitle, Amount = d.Amount, PurchaseDate = d.PurchaseDate, Status = d.Status, PaymentMethod = d.PaymentMethod };

    private static PurchaseDto PurchaseToDto(Purchase m) =>
        new(m.Id, m.StudentName, m.CourseTitle, m.Amount, m.PurchaseDate, m.Status, m.PaymentMethod);
}
