using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Interfaces;

namespace MyWebApplication.BusinessLogic.Strategy
{
    public class PaymentContext
    {
        private IPaymentService _paymentService;

        public PaymentContext(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public void SetStrategy(IPaymentService paymentService) //schimbam strategia dinamic la runtime
        {
            _paymentService = paymentService;
        }

        public void ExecutePayment(decimal amount)//executam plata folosind strategia curenta
        {
            _paymentService.Pay(amount);
        }
    }
}