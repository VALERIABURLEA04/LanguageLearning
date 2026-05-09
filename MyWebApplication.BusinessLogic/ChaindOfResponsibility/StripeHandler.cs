using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.BusinessLogic.Payments;

namespace MyWebApplication.BusinessLogic.ChainOfResponsibility
{
    public class StripeHandler : PaymentHandler
    {
        public override string Handle(PaymentRequest request)
        {
            if (request.PaymentMethod == "Stripe")
            {
                var stripe = new StripeAdapter(new StripeAPI());

                stripe.Pay(request.Amount);

                return "Processed with Stripe";
            }

            return _nextHandler?.Handle(request);
        }
    }
}
