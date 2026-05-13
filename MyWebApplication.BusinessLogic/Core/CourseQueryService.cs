using System.Collections.Generic;
using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;

namespace MyWebApplication.BusinessLogic.Core
{
    public class CourseQueryService
    {
        private readonly ICourseStore _courses;
        private readonly ILessonStore _lessons;

        public CourseQueryService(ICourseStore courses, ILessonStore lessons)
        {
            _courses = courses;
            _lessons = lessons;
        }

        public (CourseDto Course, IReadOnlyList<LessonDto> Lessons)? GetWithLessons(int id)
        {
            var course = _courses.FindById(id);
            if (course == null) return null;
            return (course, _lessons.ListByCourseTitle(course.Title));
        }
    }
}
