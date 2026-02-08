using MyWebApplication.BusinessLogic.Interfaces;
using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Core
{
    public class UserService : IUserService
    {
        private readonly List<IUser> _users = new List<IUser>();

        public void Register(IUser user)
        {
            _users.Add(user);
        }

        public IUser? GetById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<IUser> GetAll()
        {
            return _users;
        }
    }
}
