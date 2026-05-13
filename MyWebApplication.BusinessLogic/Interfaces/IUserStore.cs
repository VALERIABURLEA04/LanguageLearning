using System.Collections.Generic;
using MyWebApplication.BusinessLogic.Core.Dtos;

namespace MyWebApplication.BusinessLogic.Interfaces
{
    public interface IUserStore
    {
        UserDto? FindByEmail(string email);
        UserDto? FindById(int id);
        bool ExistsByEmail(string email);
        UserDto Create(string name, string email, string passwordHash, bool isAdmin);
        IReadOnlyList<UserDto> ListAll();
        bool SetAdminStatus(int id, bool isAdmin);
        bool Delete(int id);
    }
}
