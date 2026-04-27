using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.Domain.Interfaces;

namespace MyWebApplication.BusinessLogic.Decorator
{
    public class CertificateDecorator : CourseDecorator
    {
        public CertificateDecorator(ICourse course) : base(course) { }

        public override string GetDescription()
        {
            return _course.Title + " + Certificate";
        }
    }
}