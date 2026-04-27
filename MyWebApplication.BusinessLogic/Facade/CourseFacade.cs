using MyWebApplication.BusinessLogic.Core;
using MyWebApplication.BusinessLogic.Proxy;
using MyWebApplication.Domain.Entities;
using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApplication.BusinessLogic.Flyweight;

namespace MyWebApplication.BusinessLogic.Facade
{
    public class CourseFacade
    {
        private readonly CourseService _courseService;

        public CourseFacade()
        {
            _courseService = new CourseService();
        }

        public Course BuyAdultCourse(IPaymentService paymentService)
        {
            // Creeam curs
            var course = _courseService.CreateAdultCourse();

            //Adaugăm lecții     + flyweight
            course.Add(new Lesson 
            {
                Title = "Introduction",
                Language = LanguageFactory.GetLanguage("EN")
            });

            course.Add(new Lesson
            {
                Title = "Grammar Basics",
                Language = LanguageFactory.GetLanguage("EN")
            });

            // Plată
            _courseService.BuyCourse(course, paymentService);

            // returnăm cursul
            return course;
        }
        //proxy 
        public void AccessCourse(Course course, bool hasAccess)
        {
            ICourseAccess proxy = new CourseAccessProxy(course, hasAccess);
            proxy.AccessCourse();
        }
    }
}