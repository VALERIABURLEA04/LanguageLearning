using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.BusinessLogic.Builders;
using MyWebApplication.Domain.Entities;

namespace MyWebApplication.BusinessLogic.Core
{
    public class CourseService
    {
        public Course CreateAdultCourse()
        {
            CourseDirector director = new CourseDirector();
            AdultCourseBuilder builder = new AdultCourseBuilder();

            director.BuildAdultCourse(builder);

            Course course = builder.GetCourse();

            return course;
        }

        public Course CreateTeenCourse()
        {
            CourseDirector director = new CourseDirector();
            TeenCourseBuilder builder = new TeenCourseBuilder();

            director.BuildTeenCourse(builder);

            Course course = builder.GetCourse();

            return course;
        }

        // Prototype Pattern
        public Course CloneCourse(Course originalCourse)
        {
            Course clonedCourse = originalCourse.Clone();

            clonedCourse.Title = originalCourse.Title + " (Copy)";

            return clonedCourse;
        }
    }

}