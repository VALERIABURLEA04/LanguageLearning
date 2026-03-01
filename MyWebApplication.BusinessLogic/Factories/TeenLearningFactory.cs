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
    public class TeenLearningFactory : ILearningFactory
    {
        public ICourse CreateCourse() => new TeenEnglishCourse();
        public ILesson CreateLesson() => new TeenEnglishLesson();
        public IExercise CreateExercise() => new TeenEnglishExercise();
    }
}