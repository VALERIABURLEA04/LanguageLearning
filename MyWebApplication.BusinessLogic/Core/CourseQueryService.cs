using System.Collections.Generic;
using System.Linq;
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

        public IReadOnlyList<CourseDto> ListAll() => _courses.ListAll();

        public IReadOnlyList<CourseDto> Search(string? q)
        {
            var all = _courses.ListAll();
            if (string.IsNullOrWhiteSpace(q)) return all;
            return all.Where(c =>
                c.Title.Contains(q, System.StringComparison.OrdinalIgnoreCase) ||
                c.Description.Contains(q, System.StringComparison.OrdinalIgnoreCase) ||
                c.Level.Contains(q, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public CourseDto? FindById(int id) => _courses.FindById(id);

        public (int TotalCourses, int TotalLessons, int TotalStudents) GetPublicStats()
        {
            var courses = _courses.ListAll();
            return (courses.Count, _lessons.ListAll().Count, courses.Sum(c => c.Students));
        }

        public (CourseDto Course, IReadOnlyList<LessonDto> Lessons)? GetWithLessons(int id)
        {
            var course = _courses.FindById(id);
            if (course == null) return null;
            return (course, _lessons.ListByCourseTitle(course.Title));
        }
    }
}
