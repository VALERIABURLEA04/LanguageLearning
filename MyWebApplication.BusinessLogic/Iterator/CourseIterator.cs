using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using MyWebApplication.Domain.Entities;

namespace MyWebApplication.BusinessLogic.Iterator
{
    public class CourseIterator : ICourseIterator
    {
        private List<Course> _courses;
        private int _position = 0;

        public CourseIterator(List<Course> courses)
        {
            _courses = courses;
        }

        public bool HasNext()
        {
            return _position < _courses.Count;
        }

        public Course GetNext()
        {
            return _courses[_position++];
        }
    }
}
