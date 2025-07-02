using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace PokemonApi.Infrastructure.Auth;
public sealed class JwtIssuer
{
    readonly IConfiguration _cfg;
    public JwtIssuer(IConfiguration cfg) => _cfg = cfg;
    public string Issue(string user)
    {
        var jwt  = _cfg.GetSection("Jwt");
        var key  = Encoding.UTF8.GetBytes(jwt["Key"]!);
        var creds= new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        var tok  = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: new[]{ new Claim(ClaimTypes.Name, user) },
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(tok);
    }
}