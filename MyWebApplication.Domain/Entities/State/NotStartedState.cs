using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.State
{
    public class NotStartedState : ICourseState
    {
        public void OpenCourse(Course course)
        {
            Console.WriteLine("Course started");

            course.ChangeState(new InProgressState());
        }

        public void CompleteCourse(Course course)
        {
            Console.WriteLine("You must start the course first");
        }
    }
}