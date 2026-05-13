using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;
using MyWebApplication.BusinessLogic.Payments;

namespace MyWebApplication.BusinessLogic.Command;

/// <summary>
/// Command pattern — encapsulates a single checkout action.
/// Delegates payment to a strategy resolved via PaymentStrategyFactory (Adapter behind the scenes),
/// then persists the purchase record via IPurchaseStore.
/// </summary>
public class CheckoutCommand
{
    private readonly PaymentStrategyFactory _paymentFactory;
    private readonly IPurchaseStore         _purchases;

    public CheckoutCommand(PaymentStrategyFactory paymentFactory, IPurchaseStore purchases)
    {
        _paymentFactory = paymentFactory;
        _purchases      = purchases;
    }

    public CheckoutResult Execute(CheckoutRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) ||
            string.IsNullOrWhiteSpace(request.Email))
        {
            return new CheckoutResult { Success = false, Error = "Lipsesc datele obligatorii." };
        }

        if (request.TotalAmount <= 0)
        {
            return new CheckoutResult { Success = false, Error = "Suma de plată este invalidă." };
        }

        var amountToCharge = request.Plan?.ToLowerInvariant() == "installments"
            ? Math.Round(request.TotalAmount / 3m, 0)
            : request.TotalAmount;

        var payment = _paymentFactory.Resolve(request.PaymentMethod);
        payment.Pay(amountToCharge);

        var receiptId = $"LNG-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";

        _purchases.Create(new PurchaseDto(
            Id:            0,
            StudentName:   request.FullName,
            CourseTitle:   request.CourseTitle,
            Amount:        amountToCharge,
            PurchaseDate:  DateTime.UtcNow,
            Status:        "Completed",
            PaymentMethod: request.PaymentMethod ?? "card"
        ));

        return new CheckoutResult
        {
            Success       = true,
            ChargedAmount = amountToCharge,
            ReceiptId     = receiptId,
            Message       = $"Mulțumim, {request.FullName}! Înscrierea la \"{request.CourseTitle}\" a fost înregistrată. " +
                            $"Vei primi instrucțiunile la {request.Email}. Cod chitanță: {receiptId}."
        };
    }
}
