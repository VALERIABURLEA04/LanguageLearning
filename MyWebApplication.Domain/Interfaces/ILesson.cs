using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Interfaces
{
    public interface ILesson
    {
        string Title { get; set; }
        int DurationMinutes { get; set; }
    }
}