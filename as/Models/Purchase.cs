namespace sa.Models;

public class Purchase
{
    public int Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string Status { get; set; } = "Completed";
    public string PaymentMethod { get; set; } = "Card";
}
