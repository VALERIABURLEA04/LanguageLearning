using System.Linq;
using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;

namespace MyWebApplication.BusinessLogic.Core
{
    public class ProfileService
    {
        private readonly IUserStore _users;
        private readonly IPurchaseStore _purchases;
        private readonly ICourseStore _courses;

        public ProfileService(IUserStore users, IPurchaseStore purchases, ICourseStore courses)
        {
            _users = users;
            _purchases = purchases;
            _courses = courses;
        }

        public ProfileBundle? GetForUser(int userId)
        {
            var user = _users.FindById(userId);
            if (user == null) return null;

            var purchases = _purchases.ListByStudent(user.Name);
            var titles    = purchases.Select(p => p.CourseTitle).Distinct().ToList();
            var enrolled  = _courses.ListByTitles(titles);

            return new ProfileBundle(user, purchases, enrolled);
        }
    }
}
