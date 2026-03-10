using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Entities;

namespace MyWebApplication.BusinessLogic.Interfaces
{
    public interface ICoursePrototype
    {
        Course Clone();
    }
}