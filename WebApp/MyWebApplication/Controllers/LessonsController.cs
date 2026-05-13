using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Core;
using MyWebApplication.Domain.Entities;

namespace MyWebApplication.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsController : ControllerBase
    {
        private readonly LessonService lessonService;

        public LessonsController()
        {
            lessonService = new LessonService();
        }

        [HttpGet("{id}")]
        public ActionResult<Lesson> GetLesson(int id)
        {
            var lesson = lessonService.GetLesson(id);
            return Ok(lesson);
        }
    }
}