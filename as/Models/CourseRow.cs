namespace sa.Models;

public class CourseRow
{
    public int     Id              { get; set; }
    public string  Title           { get; set; } = string.Empty;
    public string  Language        { get; set; } = string.Empty;
    public string  Level           { get; set; } = string.Empty;
    public decimal Price           { get; set; }
    public decimal OldPrice        { get; set; }
    public int     Students        { get; set; }
    public int     Lessons         { get; set; }
    public string  Description     { get; set; } = string.Empty;
    public string  LongDescription { get; set; } = string.Empty;
    public string  ImageUrl        { get; set; } = string.Empty;
    public string  BackgroundColor { get; set; } = "#FF6B35";
    public string  Icon            { get; set; } = "📚";
    public string  Duration        { get; set; } = string.Empty;
    public string  Frequency       { get; set; } = string.Empty;
    public string  PriceNote       { get; set; } = string.Empty;
    public string  FeaturesText    { get; set; } = string.Empty;
}

public class LessonRow
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public string Status { get; set; } = "Published";
}
