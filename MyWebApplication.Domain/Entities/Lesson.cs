
using MyWebApplication.Domain.Entities.Flyweight;
using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Entities.Memento;


namespace MyWebApplication.Domain.Entities
{
    public class Lesson  : ICourse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Language Language { get; set; } //flyweight

        public LessonMemento Save()
        {
            return new LessonMemento(Title, Description); // create a memento with the current state of the lesson
        }

        public void Restore(LessonMemento memento)
        {
            Title = memento.Title;
            Description = memento.Description;
        }
        public string GetDescription()
        {
            return Title;
        }
        public void Display() 
        {
            Console.WriteLine($"Lesson: {Title}");
        }


    }
}
