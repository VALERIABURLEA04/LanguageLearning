using System.Collections.Generic;
using MyWebApplication.BusinessLogic.Core.Dtos;

namespace MyWebApplication.BusinessLogic.Interfaces
{
    public interface ILessonStore
    {
        LessonDto? FindById(int id);
        IReadOnlyList<LessonDto> ListAll();
        IReadOnlyList<LessonDto> ListByCourseTitle(string courseTitle);
        LessonDto Create(LessonDto lesson);
        bool Update(LessonDto lesson);
        bool Delete(int id);
    }
}
