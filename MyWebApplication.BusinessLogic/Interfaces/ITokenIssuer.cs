using MyWebApplication.BusinessLogic.Core.Dtos;

namespace MyWebApplication.BusinessLogic.Interfaces
{
    public interface ITokenIssuer
    {
        string Issue(UserDto user);
    }
}
