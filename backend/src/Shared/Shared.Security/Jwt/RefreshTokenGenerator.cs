using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Shared.Security.Authentication.Interfaces;
using Shared.Security.Authentication.Models;

namespace Shared.Security.Jwt;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly IOptions<JwtSettings> _jwtSettings;

    public RefreshTokenGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiryTime = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.RefreshTokenExpiryMinutes)
        };

        return refreshToken;
    }
}