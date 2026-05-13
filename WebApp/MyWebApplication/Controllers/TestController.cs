using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Facade;
using MyWebApplication.BusinessLogic.Payments;
using MyWebApplication.BusinessLogic.Proxy;
using MyWebApplication.Domain.Entities;

namespace MyWebApplication.Controllers
{
    public class TestController : Controller
    {
        public IActionResult ProxyTest()
        {
            var facade = new CourseFacade();

            var payment = new PayPalAdapter(new PayPalAPI());

            // cumpărare
            var course = facade.BuyAdultCourse(payment);

            // acces prin Proxy
            facade.AccessCourse(course, true);

            return Content("Proxy test executed. Check console output.");
        }
    }
}