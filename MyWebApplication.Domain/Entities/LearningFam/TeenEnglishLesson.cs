using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.LearningFam
{
    public class TeenEnglishLesson : ILesson
    {
        public string Title { get; set; } = "Teen Lesson 1";
        public int DurationMinutes { get; set; } = 35;
    }
}
