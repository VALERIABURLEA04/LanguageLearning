using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Entities.Flyweight;



namespace MyWebApplication.Domain.Entities.LearningFam
{
    public class TeenEnglishCourse : ICourse
    {
        public string Title { get; set; } = "English for Teens";
        public Language Language { get; set; } = null!;

        public string GetDescription()
        {
            return Title;
        }
    }
}