using MyWebApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Entities.Flyweight;


namespace MyWebApplication.Domain.Entities.Courses
{
    public class AdultEnglishCourse : Course
    {

        public override string GetDescription()
        {
            return Title;
        }
        public AdultEnglishCourse()
        {
            Title = "English for Adults";
            Type = CourseType.AdultEnglish;
        }
    }
}
