using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.LearningFam
{
    public class AdultEnglishCourse : ICourse
    {
        public string Title { get; set; } = "English for Adults";
        public string Language { get; set; } = "English";
    }
}
