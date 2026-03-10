using MyWebApplication.Domain.Entities;
using MyWebApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Builders
{
    public class CourseDirector
    {
        public void BuildAdultCourse(ICourseBuilder builder)
        {
            builder.Reset();
            builder.SetTitle("English for Adults");
            builder.SetDescription("Complete English course for adults");
            builder.SetLanguage("English");
            builder.SetLevel(LanguageLevel.B1);
            builder.SetPrice(199);
            builder.SetType(CourseType.AdultEnglish);
        }

        public void BuildTeenCourse(ICourseBuilder builder)
        {
            builder.Reset();
            builder.SetTitle("Teen English Course");
            builder.SetDescription("English course designed for teenagers");
            builder.SetLanguage("English");
            builder.SetLevel(LanguageLevel.A2);
            builder.SetPrice(99);
            builder.SetType(CourseType.TeenEnglish);
        }
    }
}