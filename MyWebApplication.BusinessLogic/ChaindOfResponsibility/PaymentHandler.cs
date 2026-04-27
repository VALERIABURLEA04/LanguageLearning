using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.ChainOfResponsibility
{
    public abstract class PaymentHandler
    {
        protected PaymentHandler _nextHandler;

        public void SetNext(PaymentHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public abstract string Handle(PaymentRequest request);
    }
}