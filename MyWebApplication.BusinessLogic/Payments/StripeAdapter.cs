using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Payments
{
    public class StripeAdapter : IPaymentService
    {
        private readonly StripeAPI _stripe;

        public StripeAdapter(StripeAPI stripe)
        {
            _stripe = stripe;
        }

        public void Pay(decimal amount)
        {
            _stripe.MakeCharge((double)amount);
        }
    }
}
