using Microsoft.AspNetCore.Mvc;

namespace Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Privacy() => View();

    public IActionResult Courses() => View();

    public IActionResult Contact() => View();

    public IActionResult Login() => View();

    public IActionResult Register() => View();
}
