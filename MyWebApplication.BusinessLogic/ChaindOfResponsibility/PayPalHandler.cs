using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.BusinessLogic.Payments;

namespace MyWebApplication.BusinessLogic.ChainOfResponsibility
{
    public class PayPalHandler : PaymentHandler
    {
        public override string Handle(PaymentRequest request)
        {
            if (request.PaymentMethod == "PayPal")
            {
                var paypal = new PayPalAdapter(new PayPalAPI());

                paypal.Pay(request.Amount);

                return "Processed with PayPal";
            }

            return _nextHandler?.Handle(request);
        }
    }
}