using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FiscalizAI.Application.Interfaces.Auth;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
