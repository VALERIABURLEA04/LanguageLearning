using MyWebApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.Courses
{
    public class AdultEnglishCourse : Course
    {
        public AdultEnglishCourse()
        {
            Title = "English for Adults";
            Type = CourseType.AdultEnglish;
        }
    }
}
