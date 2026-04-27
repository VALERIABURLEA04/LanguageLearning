using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyWebApplication.BusinessLogic.Observer
{ 
public class StudentObserver : IObserver
{
    public void Update(string message)
    {
        Console.WriteLine(message);
    }
}
} 