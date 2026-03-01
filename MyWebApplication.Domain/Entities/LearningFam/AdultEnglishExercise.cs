using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.LearningFam
{
    public class AdultEnglishExercise : IExercise
    {
        public string Title { get; set; } = "Adult Exercise 1";
        public int Points { get; set; } = 10;
    }
}