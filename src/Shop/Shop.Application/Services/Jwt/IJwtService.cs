using System.Security.Claims;

namespace Shop.Application.Services.Jwt
{
    public interface IJwtService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }
}
