using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sa.Models;

namespace Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment  _env;

    public AdminController(ApplicationDbContext db, IWebHostEnvironment env)
    {
        _db  = db;
        _env = env;
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
        ViewBag.TotalCourses    = _db.Courses.Count();
        ViewBag.TotalLessons    = _db.Lessons.Count();
        ViewBag.TotalStudents   = _db.Courses.Sum(c => c.Students);
        ViewBag.TotalRevenue    = _db.Purchases.Where(p => p.Status == "Completed").Sum(p => p.Amount);
        ViewBag.RecentPurchases = _db.Purchases.OrderByDescending(p => p.PurchaseDate).Take(5).ToList();
        return View();
    }

    // ============================== COURSES ==============================

    public IActionResult Courses() => View(_db.Courses.OrderBy(c => c.Id).ToList());

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
        _db.Courses.Add(model);
        _db.SaveChanges();
        TempData["Msg"] = $"Cursul \"{model.Title}\" a fost creat.";
        return RedirectToAction(nameof(Courses));
    }

    [HttpGet]
    public IActionResult EditCourse(int id)
    {
        var course = _db.Courses.Find(id);
        if (course == null) return NotFound();
        ViewBag.FormTitle = "Editează curs";
        return View("CourseForm", course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCourse(CourseRow model, IFormFile? imageFile, string? existingImageUrl)
    {
        var course = _db.Courses.Find(model.Id);
        if (course == null) return NotFound();

        course.Title           = model.Title;
        course.Language        = model.Language;
        course.Level           = model.Level;
        course.Price           = model.Price;
        course.OldPrice        = model.OldPrice;
        course.Lessons         = model.Lessons;
        course.Students        = model.Students;
        course.Description     = model.Description;
        course.LongDescription = model.LongDescription;
        course.BackgroundColor = model.BackgroundColor;
        course.Icon            = model.Icon;
        course.Duration        = model.Duration;
        course.Frequency       = model.Frequency;
        course.PriceNote       = model.PriceNote;
        course.FeaturesText    = model.FeaturesText;

        var newImage = await SaveImageAsync(imageFile);
        course.ImageUrl = newImage ?? existingImageUrl ?? course.ImageUrl;

        _db.SaveChanges();
        TempData["Msg"] = $"Cursul \"{course.Title}\" a fost actualizat.";
        return RedirectToAction(nameof(Courses));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteCourse(int id)
    {
        var course = _db.Courses.Find(id);
        if (course != null)
        {
            _db.Courses.Remove(course);
            _db.SaveChanges();
            TempData["Msg"] = $"Cursul \"{course.Title}\" a fost șters.";
        }
        return RedirectToAction(nameof(Courses));
    }

    // ============================== LESSONS ==============================

    public IActionResult Lessons() => View(_db.Lessons.OrderBy(l => l.Id).ToList());

    [HttpGet]
    public IActionResult CreateLesson()
    {
        ViewBag.FormTitle    = "Add Lesson";
        ViewBag.CourseTitles = _db.Courses.Select(c => c.Title).ToList();
        return View("LessonForm", new LessonRow { Status = "Published", DurationMinutes = 45 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateLesson(LessonRow model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.FormTitle    = "Add Lesson";
            ViewBag.CourseTitles = _db.Courses.Select(c => c.Title).ToList();
            return View("LessonForm", model);
        }
        _db.Lessons.Add(model);
        _db.SaveChanges();
        TempData["Msg"] = $"Lesson \"{model.Title}\" created.";
        return RedirectToAction(nameof(Lessons));
    }

    [HttpGet]
    public IActionResult EditLesson(int id)
    {
        var lesson = _db.Lessons.Find(id);
        if (lesson == null) return NotFound();
        ViewBag.FormTitle    = "Edit Lesson";
        ViewBag.CourseTitles = _db.Courses.Select(c => c.Title).ToList();
        return View("LessonForm", lesson);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditLesson(LessonRow model)
    {
        var lesson = _db.Lessons.Find(model.Id);
        if (lesson == null) return NotFound();
        lesson.Title           = model.Title;
        lesson.CourseTitle     = model.CourseTitle;
        lesson.DurationMinutes = model.DurationMinutes;
        lesson.Status          = model.Status;
        _db.SaveChanges();
        TempData["Msg"] = $"Lesson \"{lesson.Title}\" updated.";
        return RedirectToAction(nameof(Lessons));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteLesson(int id)
    {
        var lesson = _db.Lessons.Find(id);
        if (lesson != null)
        {
            _db.Lessons.Remove(lesson);
            _db.SaveChanges();
            TempData["Msg"] = $"Lesson \"{lesson.Title}\" deleted.";
        }
        return RedirectToAction(nameof(Lessons));
    }

    // ============================== PURCHASES ==============================

    public IActionResult Purchases() => View(_db.Purchases.OrderByDescending(p => p.PurchaseDate).ToList());

    [HttpGet]
    public IActionResult CreatePurchase()
    {
        ViewBag.FormTitle    = "Add Purchase";
        ViewBag.CourseTitles = _db.Courses.Select(c => c.Title).ToList();
        return View("PurchaseForm", new Purchase { PurchaseDate = DateTime.Now, Status = "Completed", PaymentMethod = "Stripe" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreatePurchase(Purchase model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.FormTitle    = "Add Purchase";
            ViewBag.CourseTitles = _db.Courses.Select(c => c.Title).ToList();
            return View("PurchaseForm", model);
        }
        _db.Purchases.Add(model);
        _db.SaveChanges();
        TempData["Msg"] = $"Purchase #{model.Id} created.";
        return RedirectToAction(nameof(Purchases));
    }

    [HttpGet]
    public IActionResult EditPurchase(int id)
    {
        var purchase = _db.Purchases.Find(id);
        if (purchase == null) return NotFound();
        ViewBag.FormTitle    = "Edit Purchase";
        ViewBag.CourseTitles = _db.Courses.Select(c => c.Title).ToList();
        return View("PurchaseForm", purchase);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditPurchase(Purchase model)
    {
        var purchase = _db.Purchases.Find(model.Id);
        if (purchase == null) return NotFound();
        purchase.StudentName   = model.StudentName;
        purchase.CourseTitle   = model.CourseTitle;
        purchase.Amount        = model.Amount;
        purchase.PurchaseDate  = model.PurchaseDate;
        purchase.Status        = model.Status;
        purchase.PaymentMethod = model.PaymentMethod;
        _db.SaveChanges();
        TempData["Msg"] = $"Purchase #{purchase.Id} updated.";
        return RedirectToAction(nameof(Purchases));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePurchase(int id)
    {
        var purchase = _db.Purchases.Find(id);
        if (purchase != null)
        {
            _db.Purchases.Remove(purchase);
            _db.SaveChanges();
            TempData["Msg"] = $"Purchase #{id} deleted.";
        }
        return RedirectToAction(nameof(Purchases));
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
}
