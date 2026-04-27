using MyWebApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Entities.Flyweight;


namespace MyWebApplication.Domain.Entities.Courses
    {
        public class TeenEnglishCourse : Course
        {

        public string GetDescription()
        {
            return Title;
        }
        public TeenEnglishCourse()
            {
                Title = "English for Teenagers";
                Type = CourseType.TeenEnglish;


            }
        }
    }
