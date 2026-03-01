using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.LearningFam
{
    public class AdultEnglishLesson : ILesson
    {
        public string Title { get; set; } = "Adult Lesson 1";
        public int DurationMinutes { get; set; } = 45;
    }
}
