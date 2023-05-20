using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace CAEVSYNC.Auth.Services;

public class JwtTokenService
{
    private const int MINUTES_TO_EXPIRATION = 3000;

    public string CreateToken(IdentityUser user)
    {
        var expireAt = DateTime.UtcNow.AddMinutes(MINUTES_TO_EXPIRATION);
        var token = CreateJwtToken(CreateClaims(user), CreateSigningCredentials(), expireAt);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(
        List<Claim> claims,
        SigningCredentials credentials,
        DateTime expiration)
    {
        return new JwtSecurityToken(
            Environment.GetEnvironmentVariable("AUTH_VALID_ISSUER"),
            Environment.GetEnvironmentVariable("AUTH_VALID_AUDIENCE"),
            claims,
            expires: expiration,
            signingCredentials: credentials
        );
    }

    private List<Claim> CreateClaims(IdentityUser user)
    { 
        var claims = new List<Claim> 
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName), 
            new Claim(ClaimTypes.Email, user.Email)
        };
        return claims;
    }
    
    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AUTH_ISSUER_SIGNING_KEY"))
            ),
            SecurityAlgorithms.HmacSha256
        );
    }
}