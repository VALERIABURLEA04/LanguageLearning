using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Command
{
    public class CommandInvoker
    {
        private Stack<ICommand> _history = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _history.Push(command);
        }

        public void Undo()
        {
            if (_history.Count > 0)
            {
                var cmd = _history.Pop();
                cmd.Undo();
            }
        }
    }
}