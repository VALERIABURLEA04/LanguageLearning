using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Interfaces;

namespace MyWebApplication.BusinessLogic.Decorator
{
    public class SupportDecorator : CourseDecorator
    {
        public SupportDecorator(ICourse course) : base(course) { }

        public override string GetDescription()
        {
            return _course.Title + " + Support";
        }
    }
}