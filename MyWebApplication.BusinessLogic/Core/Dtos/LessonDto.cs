namespace MyWebApplication.BusinessLogic.Core.Dtos
{
    public record LessonDto(
        int Id,
        string Title,
        string CourseTitle,
        int DurationMinutes,
        string Status);
}
