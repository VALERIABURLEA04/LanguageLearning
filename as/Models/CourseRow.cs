namespace sa.Models;

public class CourseRow
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Students { get; set; }
    public int Lessons { get; set; }
}

public class LessonRow
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public string Status { get; set; } = "Published";
}
