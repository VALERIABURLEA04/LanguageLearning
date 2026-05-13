using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;

namespace MyWebApplication.BusinessLogic.Command
{
    /// <summary>
    /// Command pattern — encapsulates a single enrolment action against the course store.
    /// </summary>
    public class EnrollStudentCommand
    {
        private readonly ICourseStore _courses;

        public EnrollStudentCommand(ICourseStore courses) => _courses = courses;

        public EnrollResult Execute(int courseId)
        {
            var course = _courses.FindById(courseId);
            if (course == null)
                return new EnrollResult(false, null, "Course not found.");

            _courses.IncrementStudents(courseId);
            return new EnrollResult(true, course.Title, null);
        }
    }
}
