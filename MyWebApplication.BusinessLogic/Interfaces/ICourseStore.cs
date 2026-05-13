using System.Collections.Generic;
using MyWebApplication.BusinessLogic.Core.Dtos;

namespace MyWebApplication.BusinessLogic.Interfaces
{
    public interface ICourseStore
    {
        CourseDto? FindById(int id);
        IReadOnlyList<CourseDto> ListAll();
        IReadOnlyList<CourseDto> ListByTitles(IReadOnlyList<string> titles);
        bool IncrementStudents(int courseId);
        CourseDto Create(CourseDto course);
        bool Update(CourseDto course);
        bool Delete(int id);
    }
}
