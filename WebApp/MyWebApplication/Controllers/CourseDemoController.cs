using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Factories;
using MyWebApplication.BusinessLogic.Interfaces;
using MyWebApplication.Domain.Enums;
using MyWebApplication.BusinessLogic.Core;
using MyWebApplication.Domain.Entities;

namespace MyWebApplication.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly CourseService courseService;

        public CourseController()
        {
            courseService = new CourseService();
        }

        [HttpGet("adult")]
        public ActionResult<Course> GetAdultCourse()
        {
            var course = courseService.CreateAdultCourse();
            return Ok(course);
        }

        [HttpGet("teen")]
        public ActionResult<Course> GetTeenCourse()
        {
            var course = courseService.CreateTeenCourse();
            return Ok(course);
        }


        ///prototipul pentru clonarea unui curs existent

        [HttpGet("clone-adult")]
        public ActionResult<Course> CloneAdultCourse()
        {
            var service = new CourseService();

            // creăm un curs folosind Builder
            Course original = service.CreateAdultCourse();

            // clonăm cursul folosind Prototype
            Course clone = service.CloneCourse(original);

            return Ok(clone);
        }
    }
}

//namespace MyWebApplication.Controllers
//{
//    public class CourseDemoController : Controller
//    {
//        public IActionResult Index()
//        {
//            ICourseFactory factory = new CourseFactory();

//            var course1 = factory.CreateCourse(CourseType.AdultEnglish);
//            var course2 = factory.CreateCourse(CourseType.Cambridge);
//            var course3 = factory.CreateCourse(CourseType.BusinessEnglish);

//            ViewBag.Course1 = course1.Title;
//            ViewBag.Course2 = course2.Title;
//            ViewBag.Course3 = course3.Title;

//            return View();
//        }
//    }
//}

