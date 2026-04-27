using MyWebApplication.BusinessLogic.Observer;
using MyWebApplication.Domain.Entities;
using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Observer
{
    public class ObservableCourse : Course, ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var obs in _observers)
            {
                obs.Update(message);
            }
        }

        // acțiune reală
        public void AddLessonWithNotify(ICourse lesson)
        {
            Add(lesson); // din Composite
            Notify($"New lesson added to course {Title}");
        }
    }
}