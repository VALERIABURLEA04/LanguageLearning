using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.ChainOfResponsibility
{
    public class PaymentRequest
    {
        public string StudentName { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; }
    }
}