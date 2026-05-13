namespace MyWebApplication.BusinessLogic.Core.Dtos
{
    public record PurchaseDto(
        int Id,
        string StudentName,
        string CourseTitle,
        decimal Amount,
        System.DateTime PurchaseDate,
        string Status,
        string PaymentMethod);
}
