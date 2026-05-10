using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.State
{
    public class InProgressState : ICourseState
    {
        public void OpenCourse(Course course)
        {
            Console.WriteLine("Continue learning");
        }

        public void CompleteCourse(Course course)
        {
            Console.WriteLine("Course completed");

            course.ChangeState(new CompletedState());
        }
    }
}