using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Interfaces;
using MyWebApplication.Domain.Entities.Flyweight;

namespace MyWebApplication.BusinessLogic.Decorator
{
    public abstract class CourseDecorator : ICourse
    {
        protected ICourse _course;

        public CourseDecorator(ICourse course)
        {
            _course = course;
        }

        public virtual string Title   //doar transmite mai departe
        {
            get => _course.Title;
            set => _course.Title = value;
        }

        public virtual Language Language
        {
            get => _course.Language;
            set => _course.Language = value;
        }

        // metodă extra pentru info
        public virtual string GetDescription()
        {
            return _course.Title;
        }
    }
}
