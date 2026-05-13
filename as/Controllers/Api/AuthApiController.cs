using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Facade;

namespace Controllers.Api;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthApiController : ControllerBase
{
    private readonly AuthFacade _auth;

    public AuthApiController(AuthFacade auth) => _auth = auth;

    public class LoginInput
    {
        public string Email    { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterInput
    {
        public string Name     { get; set; } = string.Empty;
        public string Email    { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        /// <summary>Defaults to false. Set true only for seeding/testing admin accounts.</summary>
        [DefaultValue(false)]
        public bool   IsAdmin  { get; set; } = false;
    }

    /// <summary>Logs in a user. Returns JWT + welcome message on success.</summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginInput input)
    {
        var result = _auth.Login(input.Email, input.Password);
        if (!result.Success)
            return Unauthorized(new { error = result.Error });

        return Ok(new
        {
            user = new
            {
                result.User!.Id,
                result.User.Name,
                result.User.Email,
                result.User.IsAdmin,
                result.User.CreatedAt
            },
            token          = result.Token,
            welcomeMessage = result.Outcome?.WelcomeMessage
        });
    }

    /// <summary>Registers a new user. IsAdmin defaults to false.</summary>
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterInput input)
    {
        var result = _auth.Register(input.Name, input.Email, input.Password, input.IsAdmin);
        if (!result.Success)
            return BadRequest(new { error = result.Error });

        return Ok(new
        {
            user = new
            {
                result.User!.Id,
                result.User.Name,
                result.User.Email,
                result.User.IsAdmin,
                result.User.CreatedAt
            },
            token          = result.Token,
            welcomeMessage = result.Outcome?.WelcomeMessage
        });
    }
}
