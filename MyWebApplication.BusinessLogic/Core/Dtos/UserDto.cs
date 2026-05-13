namespace MyWebApplication.BusinessLogic.Core.Dtos
{
    public record UserDto(
        int Id,
        string Name,
        string Email,
        string PasswordHash,
        bool IsAdmin,
        System.DateTime CreatedAt);
}
