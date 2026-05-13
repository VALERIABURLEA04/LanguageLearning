using MyWebApplication.BusinessLogic.Strategy;

namespace MyWebApplication.BusinessLogic.Core.Dtos
{
    public record AuthResult(
        bool Success,
        UserDto? User,
        string? Token,
        LoginOutcome? Outcome,
        string? Error);
}
