using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Interfaces
{
    public interface ILearningFactory
    {
        ICourse CreateCourse();
        ILesson CreateLesson();
        IExercise CreateExercise();
    }
}
