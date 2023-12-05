using Shared.Security.Authentication.Models;

namespace Shared.Security.Authentication.Interfaces;

public interface IRefreshTokenGenerator
{
    public RefreshToken GenerateRefreshToken();
}