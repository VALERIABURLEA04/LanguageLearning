using MyWebApplication.Domain.Entities;
using MyWebApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Builders
{
    public interface ICourseBuilder
    {
        void Reset();

        void SetTitle(string title);

        void SetDescription(string description);

        void SetLanguage(string language);

        void SetLevel(LanguageLevel level);

        void SetPrice(decimal price);

        void SetType(CourseType type);

        Course GetCourse();
    }
}