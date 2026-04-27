using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.BusinessLogic.Bridge;

namespace MyWebApplication.BusinessLogic.Bridge
{
    public abstract class CourseBridge
    {
        protected ICourseDelivery _delivery;

        public CourseBridge(ICourseDelivery delivery)
        {
            _delivery = delivery;
        }

        public abstract string GetCourseInfo();
    }
}