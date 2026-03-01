using MyWebApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace MyWebApplication.Domain.Entities.Courses
    {
        public class CambridgeCourse : Course
        {
            public CambridgeCourse()
            {
                Title = "Cambridge Preparation";
                Type = CourseType.Cambridge;
            }
        }
    }
