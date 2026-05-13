using MyWebApplication.Domain.Interfaces;

namespace MyWebApplication.BusinessLogic.Payments;

/// <summary>
/// Factory + Strategy — resolves the appropriate IPaymentService (Adapter)
/// at runtime based on the user-selected payment method.
/// </summary>
public class PaymentStrategyFactory
{
    private readonly StripeAdapter _stripe;
    private readonly PayPalAdapter _paypal;

    public PaymentStrategyFactory(StripeAdapter stripe, PayPalAdapter paypal)
    {
        _stripe = stripe;
        _paypal = paypal;
    }

    public IPaymentService Resolve(string method) => method?.ToLowerInvariant() switch
    {
        "paypal" => _paypal,
        "stripe" => _stripe,
        "card"   => _stripe,
        "bank"   => _stripe,
        _         => _stripe
    };
}
