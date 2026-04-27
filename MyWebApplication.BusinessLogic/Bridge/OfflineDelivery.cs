using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MyWebApplication.BusinessLogic.Bridge
{
    public class OfflineDelivery : ICourseDelivery
    {
        public string Deliver()
        {
            return "Delivered in classroom";
        }
    }
}