using MockInterview.Identity.Application.Common.Models.Users;
using MockInterview.Identity.Domain.Entities.Users;

namespace MockInterview.Identity.Application.Authentication.Models;

public record AuthenticationResult(UserDto User, string AccessToken, string RefreshToken,
    DateTime RefreshTokenExpiryTime, bool IsNewUser = false);