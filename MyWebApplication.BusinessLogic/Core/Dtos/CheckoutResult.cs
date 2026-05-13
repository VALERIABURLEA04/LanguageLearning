namespace MyWebApplication.BusinessLogic.Core.Dtos;

public class CheckoutResult
{
    public bool   Success     { get; set; }
    public string Message     { get; set; } = string.Empty;
    public string ReceiptId   { get; set; } = string.Empty;
    public decimal ChargedAmount { get; set; }
    public string? Error      { get; set; }
}
