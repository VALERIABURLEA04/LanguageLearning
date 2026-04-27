using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Entities;

namespace MyWebApplication.BusinessLogic.Proxy
{
    public class CourseAccessProxy : ICourseAccess
    {
        private readonly RealCourseAccess _realAccess;
        private readonly bool _hasAccess; // are userul dreptul fie nu de acces

        public CourseAccessProxy(Course course, bool hasAccess) //constructor
        {
            _realAccess = new RealCourseAccess(course);
            _hasAccess = hasAccess;
        }

        public void AccessCourse()
        {
            if (_hasAccess)
            {
                Console.WriteLine("Access granted.");
                _realAccess.AccessCourse();
            }
            else
            {
                Console.WriteLine("Access denied. Please purchase the course.");
            }
        }
    }
}
