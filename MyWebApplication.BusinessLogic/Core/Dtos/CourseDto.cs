namespace MyWebApplication.BusinessLogic.Core.Dtos
{
    public record CourseDto(
        int Id,
        string Title,
        string Language,
        string Level,
        decimal Price,
        int Students,
        int Lessons)
    {
        public decimal OldPrice        { get; init; }
        public string  Description     { get; init; } = string.Empty;
        public string  LongDescription { get; init; } = string.Empty;
        public string  ImageUrl        { get; init; } = string.Empty;
        public string  BackgroundColor { get; init; } = "#FF6B35";
        public string  Icon            { get; init; } = "📚";
        public string  Duration        { get; init; } = string.Empty;
        public string  Frequency       { get; init; } = string.Empty;
        public string  PriceNote       { get; init; } = string.Empty;
        public string  FeaturesText    { get; init; } = string.Empty;
    }
}
