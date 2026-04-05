using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Payments
{
    public class PayPalAdapter : IPaymentService
    {
        private readonly PayPalAPI _paypal;

        public PayPalAdapter(PayPalAPI paypal)
        {
            _paypal = paypal;
        }

        public void Pay(decimal amount)
        {
            _paypal.SendPayment((double)amount);
        }
    }
}

