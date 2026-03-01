using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.LearningFam
{
    public class TeenEnglishCourse : ICourse
    {
        public string Title { get; set; } = "English for Teens";
        public string Language { get; set; } = "English";
    }
}