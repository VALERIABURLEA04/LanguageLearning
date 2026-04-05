using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Payments
{
  
    public class StripeAPI
    {
        public void MakeCharge(double amount)
        {
            Console.WriteLine($"Paid {amount} using Stripe");
        }
    }
}
