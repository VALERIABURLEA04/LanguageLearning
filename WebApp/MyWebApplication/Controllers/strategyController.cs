using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Strategy;
using MyWebApplication.BusinessLogic.Payments;

namespace MyWebApplication.Controllers
{
    public class StrategyController : Controller
    {
        public IActionResult TestStrategy()
        {
            var paypal = new PaymentContext(new PayPalAdapter(new PayPalAPI()));
            paypal.ExecutePayment(100);

            var stripe = new PaymentContext(new StripeAdapter(new StripeAPI()));
            stripe.ExecutePayment(200);

            return Content("Strategy works with existing PaymentService!");
        }
    }
}