using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Entities;

namespace MyWebApplication.BusinessLogic.Command
{
    public class EnrollCommand : ICommand
    {
        private string _student;

        public EnrollCommand(string student)
        {
            _student = student;
        }

        public void Execute()
        {
            Console.WriteLine($"{_student} enrolled");
        }

        public void Undo()
        {
            Console.WriteLine($"{_student} unenrolled");
        }
    }
}