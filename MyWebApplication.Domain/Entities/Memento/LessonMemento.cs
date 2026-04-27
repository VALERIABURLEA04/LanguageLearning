using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Entities.Memento;


namespace MyWebApplication.Domain.Entities.Memento
{
    public class LessonMemento
    {
        public string Title { get; }
        public string Description { get; }

        public LessonMemento(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}

