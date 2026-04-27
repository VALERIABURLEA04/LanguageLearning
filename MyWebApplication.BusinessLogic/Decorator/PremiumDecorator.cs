using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Interfaces;

namespace MyWebApplication.BusinessLogic.Decorator
{
    public class PremiumDecorator : CourseDecorator
    {
        public PremiumDecorator(ICourse course) : base(course) { }

        public override string GetDescription()
        {
            return _course.Title + " + Premium Access";
        }
    }
}
