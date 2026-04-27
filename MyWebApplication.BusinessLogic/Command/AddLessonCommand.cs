using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Entities;
using MyWebApplication.Domain.Interfaces;

namespace MyWebApplication.BusinessLogic.Command
{
    public class AddLessonCommand : ICommand
    {
        private Course _course;
        private ICourse _lesson;

        public AddLessonCommand(Course course, ICourse lesson)
        {
            _course = course;
            _lesson = lesson;
        }

        public void Execute()
        {
            _course.Add(_lesson);
        }

        public void Undo()
        {
            _course.Remove(_lesson);
        }
    }
}