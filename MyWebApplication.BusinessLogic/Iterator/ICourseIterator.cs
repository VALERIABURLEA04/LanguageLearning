using MyWebApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Iterator
{
    public interface ICourseIterator
    {
        bool HasNext();
        Course GetNext();
    }
}