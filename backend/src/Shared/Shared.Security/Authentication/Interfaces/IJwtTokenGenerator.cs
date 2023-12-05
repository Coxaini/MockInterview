using System.Security.Claims;
using Shared.Security.Authentication.Models;

namespace Shared.Security.Authentication.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserClaims userClaims);
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}