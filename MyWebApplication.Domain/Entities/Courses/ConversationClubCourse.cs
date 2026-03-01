using MyWebApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.Courses
{
    public class ConversationClubCourse : Course
    {
        public ConversationClubCourse()
        {
            Title = "Conversation Club";
            Type = CourseType.ConversationClub;
        }
    }
}