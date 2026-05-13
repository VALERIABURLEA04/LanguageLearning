using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Core;
using MyWebApplication.BusinessLogic.Factories;
using MyWebApplication.Domain.Enums;

namespace MyWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FactoryTestController : ControllerBase
    {
        [HttpGet("course/{type}")]
        public IActionResult CreateCourse(CourseType type)
        {
            var factory = new CourseFactory();

            var course = factory.CreateCourse(type);

            return Ok(new
            {
                CourseName = course.Title,
                CourseType = course.Type
            });
        }
    }
}