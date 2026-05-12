using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using sa.Models;

namespace sa.Services;

public class TokenService
{
    private readonly IConfiguration _cfg;
    public TokenService(IConfiguration cfg) => _cfg = cfg;

    public string Generate(User user)
    {
        var key       = _cfg["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key missing");
        var issuer    = _cfg["Jwt:Issuer"];
        var audience  = _cfg["Jwt:Audience"];
        var expiryMin = int.TryParse(_cfg["Jwt:ExpiryMinutes"], out var m) ? m : 60;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name,               user.Name),
            new Claim(ClaimTypes.Role,               user.IsAdmin ? "Admin" : "User"),
            new Claim("isAdmin",                     user.IsAdmin.ToString().ToLower())
        };

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:             issuer,
            audience:           audience,
            claims:             claims,
            expires:            DateTime.UtcNow.AddMinutes(expiryMin),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
