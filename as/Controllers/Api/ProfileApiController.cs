using Microsoft.AspNetCore.Mvc;
using MyWebApplication.BusinessLogic.Core;

namespace Controllers.Api;

[ApiController]
[Route("api/profile")]
[Produces("application/json")]
public class ProfileApiController : ControllerBase
{
    private readonly ProfileService _profile;

    public ProfileApiController(ProfileService profile) => _profile = profile;

    /// <summary>Get the profile bundle (user + purchases + enrolled courses) for a user.</summary>
    [HttpGet("{userId:int}")]
    public IActionResult Get(int userId)
    {
        var bundle = _profile.GetForUser(userId);
        if (bundle is null) return NotFound(new { error = $"User {userId} not found." });
        return Ok(bundle);
    }
}
