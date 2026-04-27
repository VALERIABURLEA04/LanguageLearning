using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using MyWebApplication.Domain.Entities;

namespace MyWebApplication.BusinessLogic.Iterator
{
    public class CourseCollection
    {
        private List<Course> _courses = new List<Course>();

        public void AddCourse(Course course)
        {
            _courses.Add(course);
        }

        public ICourseIterator CreateIterator()
        {
            return new CourseIterator(_courses);
        }
    }
}
