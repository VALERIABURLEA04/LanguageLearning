using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.LearningFam
{
    public class TeenEnglishExercise : IExercise
    {
        public string Title { get; set; } = "Teen Exercise 1";
        public int Points { get; set; } = 8;
    }
}