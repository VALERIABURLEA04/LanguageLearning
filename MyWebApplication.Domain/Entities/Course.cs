using MyWebApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Language { get; set; } = string.Empty;

        public LanguageLevel Level { get; set; }

        public decimal Price { get; set; }

        public CourseType Type { get; set; }
    }
}