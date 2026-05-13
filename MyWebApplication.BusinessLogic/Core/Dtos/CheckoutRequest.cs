namespace MyWebApplication.BusinessLogic.Core.Dtos;

public class CheckoutRequest
{
    public string CategorySlug { get; set; } = string.Empty;
    public string CourseTitle  { get; set; } = string.Empty;
    public string FullName     { get; set; } = string.Empty;
    public string Email        { get; set; } = string.Empty;
    public string Phone        { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = "card";
    public string Plan          { get; set; } = "full";
    public decimal TotalAmount  { get; set; }
}
