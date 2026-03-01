using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Entities;
using MyWebApplication.Domain.Enums;

namespace MyWebApplication.BusinessLogic.Interfaces
{
    public interface ICourseFactory
    {
        Course CreateCourse(CourseType type);
    }
}

