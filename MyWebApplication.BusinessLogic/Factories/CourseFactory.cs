using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.BusinessLogic.Interfaces;
using MyWebApplication.Domain.Entities;
using MyWebApplication.Domain.Entities.Courses;
using MyWebApplication.Domain.Enums;

namespace MyWebApplication.BusinessLogic.Factories
{
    public class CourseFactory : ICourseFactory
    {
        public Course CreateCourse(CourseType type)
        {
            switch (type)
            {
                case CourseType.AdultEnglish:
                    return new AdultEnglishCourse();

                case CourseType.TeenEnglish:
                    return new TeenEnglishCourse();

                case CourseType.Cambridge:
                    return new CambridgeCourse();

                case CourseType.BusinessEnglish:
                    return new BusinessEnglishCourse();

                case CourseType.ConversationClub:
                    return new ConversationClubCourse();

                default:
                    throw new ArgumentException("Unknown course type");
            }
        }
    }
}
