using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Bridge;

namespace MyWebApplication.Controllers
{
    public class BridgeController : Controller
    {
        public IActionResult TestBridge()
        {
            var course1 = new EnglishCourseBridge(new OnlineDelivery());
            var course2 = new EnglishCourseBridge(new OfflineDelivery());

            var result = "";
            result += course1.GetCourseInfo() + "\n";
            result += course2.GetCourseInfo();

            return Content(result);
        }
    }
}