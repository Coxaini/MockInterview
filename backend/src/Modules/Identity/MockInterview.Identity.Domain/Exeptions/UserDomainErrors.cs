using FluentResults;
using Shared.Domain.Errors;

namespace MockInterview.Identity.Domain.Errors;

public static class UserDomainErrors
{
    public static readonly IError DublicatedCredantials =
        new DomainError($"User already has this credential", nameof(DublicatedCredantials));

    public static readonly IError UserMustHaveAtLeastOneCredential =
        new DomainError($"User must have at least one credential", nameof(UserMustHaveAtLeastOneCredential));

    public static readonly IError RefreshTokenMustBeDifferent =
        new DomainError($"Refresh token must be different", nameof(RefreshTokenMustBeDifferent));

    public static readonly IError InvalidRefreshTokenExpiryTime =
        new DomainError($"Invalid refresh token expiry time", nameof(InvalidRefreshTokenExpiryTime));

    public static readonly IError InvalidRefreshToken =
        new DomainError($"Invalid refresh token", nameof(InvalidRefreshToken));
}