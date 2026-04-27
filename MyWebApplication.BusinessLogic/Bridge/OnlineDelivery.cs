using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MyWebApplication.BusinessLogic.Bridge
{
    public class OnlineDelivery : ICourseDelivery
    {
        public string Deliver()
        {
            return "Delivered online via platform";
        }
    }
}

