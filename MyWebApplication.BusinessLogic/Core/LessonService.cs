using MyWebApplication.BusinessLogic.Logging;
using MyWebApplication.Domain.Entities;
using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Core
{
  public class LessonService
    {
        public   Lesson GetLesson(int id)
        {
            Logger logger = Logger.GetInstance();

            logger.Log($"Lesson requested with ID {id}");

            // simulăm obținerea lecției
            Lesson lesson = new Lesson
            {
                Id = id,
                Title = "Sample Lesson"
            };

            logger.Log($"Lesson {lesson.Title} returned");

            return lesson;
        }
    }
}
