using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Payments;

namespace MyWebApplication.BusinessLogic.Command;

/// <summary>
/// Command pattern — encapsulates a single checkout action.
/// Delegates payment to a strategy resolved via PaymentStrategyFactory (Adapter behind the scenes).
/// </summary>
public class CheckoutCommand
{
    private readonly PaymentStrategyFactory _paymentFactory;

    public CheckoutCommand(PaymentStrategyFactory paymentFactory)
    {
        _paymentFactory = paymentFactory;
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

        // Plan affects the amount charged now ("installments" = first instalment only).
        var amountToCharge = request.Plan?.ToLowerInvariant() == "installments"
            ? Math.Round(request.TotalAmount / 3m, 0)
            : request.TotalAmount;

        // Strategy/Adapter selection through factory.
        var payment = _paymentFactory.Resolve(request.PaymentMethod);
        payment.Pay(amountToCharge);

        var receiptId = $"LNG-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";

        return new CheckoutResult
        {
            Success        = true,
            ChargedAmount  = amountToCharge,
            ReceiptId      = receiptId,
            Message        = $"Mulțumim, {request.FullName}! Înscrierea la „{request.CourseTitle}\" a fost înregistrată. " +
                             $"Vei primi instrucțiunile la {request.Email}. Cod chitanță: {receiptId}."
        };
    }
}
