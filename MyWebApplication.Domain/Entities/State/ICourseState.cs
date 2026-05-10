using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.State
{//definește comportamentul tuturor stărilor.
    public interface ICourseState
    {
        void OpenCourse(Course course);

        void CompleteCourse(Course course);
    }
}