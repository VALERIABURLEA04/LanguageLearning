using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Payments
{
    public class PayPalAPI
    {
        public void SendPayment(double amount)
        {
            Console.WriteLine($"Paid {amount} using PayPal");
        }
    }
}
