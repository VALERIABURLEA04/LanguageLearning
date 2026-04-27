using MyWebApplication.Domain.Entities.Flyweight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Interfaces
{
    public interface ICourse
    {
        string Title { get; set; }
        Language Language { get; set; }

        string GetDescription();
        // void Display();   //for composite 

    }
}
