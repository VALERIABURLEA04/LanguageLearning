using MyWebApplication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        void Register(IUser user);

        IUser? GetById(int id);

        IEnumerable<IUser> GetAll();
    }
}
