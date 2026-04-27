using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using MyWebApplication.Domain.Entities.Memento;


namespace MyWebApplication.BusinessLogic.Memento
{
    public class LessonHistory
    {
        private Stack<LessonMemento> _history = new Stack<LessonMemento>();

        public void SaveState(LessonMemento memento)
        {
            _history.Push(memento); //salvarea stării curente a lecției în istoric (din lessonmememnto 
        }

        public LessonMemento Undo()
        {
            return _history.Count > 0 ? _history.Pop() : null; //scoate ultima stare salvată din istoric și o returnează
        }
    }
}
