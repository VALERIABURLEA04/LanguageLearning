using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MyWebApplication.BusinessLogic.Bridge
{
    public class EnglishCourseBridge : CourseBridge
    {
        public EnglishCourseBridge(ICourseDelivery delivery)
            : base(delivery) { }

        public override string GetCourseInfo()
        {
            return "English Course - " + _delivery.Deliver();
        }
    }
}