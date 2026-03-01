using MyWebApplication.BusinessLogic.Interfaces;
using MyWebApplication.Domain.Entities.LearningFam;
using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Factories
{
    public class AdultLearningFactory : ILearningFactory
    {
        public ICourse CreateCourse() => new AdultEnglishCourse();
        public ILesson CreateLesson() => new AdultEnglishLesson();
        public IExercise CreateExercise() => new AdultEnglishExercise();
    }
}