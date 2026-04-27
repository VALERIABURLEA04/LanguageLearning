using MyWebApplication.BusinessLogic.Flyweight;
using MyWebApplication.Domain.Entities;
using MyWebApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Builders
{
    public class AdultCourseBuilder : ICourseBuilder
    {
        private Course course;

        public AdultCourseBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            course = new Course();
        }

        public void SetTitle(string title)
        {
            course.Title = title;
        }

        public void SetDescription(string description)
        {
            course.Description = description;
        }

        public void SetLanguage(string language)
        {
            course.Language = LanguageFactory.GetLanguage(language);
        }

        public void SetLevel(LanguageLevel level)
        {
            course.Level = level;
        }

        public void SetPrice(decimal price)
        {
            course.Price = price;
        }

        public void SetType(CourseType type)
        {
            course.Type = type;
        }

        public Course GetCourse()
        {
            Course result = course;
            Reset();
            return result;
        }
    }
}