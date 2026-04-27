using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Entities;

namespace MyWebApplication.BusinessLogic.Proxy
{
    public class RealCourseAccess : ICourseAccess
    {
        private readonly Course _course;

        public RealCourseAccess(Course course)
        {
            _course = course;
        }

        public void AccessCourse()
        {
            Console.WriteLine($"Accessing course: {_course.Title}");
        }
    }
}