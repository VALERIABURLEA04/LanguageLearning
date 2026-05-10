using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.State
{
    public class CompletedState : ICourseState
    {
        public void OpenCourse(Course course)
        {
            Console.WriteLine("Course already completed");
        }

        public void CompleteCourse(Course course)
        {
            Console.WriteLine("Course already completed");
        }
    }
}