using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;
using MyWebApplication.BusinessLogic.Payments;

namespace MyWebApplication.BusinessLogic.Command;

public class CheckoutCommand : ICommand
{
    private readonly PaymentStrategyFactory _paymentFactory;
    private readonly IPurchaseStore         _purchases;

    private CheckoutRequest? _request;
    private int              _createdPurchaseId;

    public CheckoutResult Result { get; private set; } = new();

    public CheckoutCommand(PaymentStrategyFactory paymentFactory, IPurchaseStore purchases)
    {
        _paymentFactory = paymentFactory;
        _purchases      = purchases;
    }

    public void Setup(CheckoutRequest request) => _request = request;

    public void Execute()
    {
        if (_request is null)
        {
            Result = new CheckoutResult { Success = false, Error = "Comanda nu a fost inițializată." };
            return;
        }

        if (string.IsNullOrWhiteSpace(_request.FullName) || string.IsNullOrWhiteSpace(_request.Email))
        {
            Result = new CheckoutResult { Success = false, Error = "Lipsesc datele obligatorii." };
            return;
        }

        if (_request.TotalAmount <= 0)
        {
            Result = new CheckoutResult { Success = false, Error = "Suma de plată este invalidă." };
            return;
        }

        var amountToCharge = _request.Plan?.ToLowerInvariant() == "installments"
            ? Math.Round(_request.TotalAmount / 3m, 0)
            : _request.TotalAmount;

        var payment = _paymentFactory.Resolve(_request.PaymentMethod);
        payment.Pay(amountToCharge);

        var receiptId = $"LNG-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";

        var created = _purchases.Create(new PurchaseDto(
            Id:            0,
            StudentName:   _request.FullName,
            CourseTitle:   _request.CourseTitle,
            Amount:        amountToCharge,
            PurchaseDate:  DateTime.UtcNow,
            Status:        "Completed",
            PaymentMethod: _request.PaymentMethod ?? "card"
        ));

        _createdPurchaseId = created.Id;

        Result = new CheckoutResult
        {
            Success       = true,
            ChargedAmount = amountToCharge,
            ReceiptId     = receiptId,
            Message       = $"Mulțumim, {_request.FullName}! Înscrierea la \"{_request.CourseTitle}\" a fost înregistrată. " +
                            $"Vei primi instrucțiunile la {_request.Email}. Cod chitanță: {receiptId}."
        };
    }

    public void Undo()
    {
        if (_createdPurchaseId > 0)
            _purchases.Delete(_createdPurchaseId);
    }
}
