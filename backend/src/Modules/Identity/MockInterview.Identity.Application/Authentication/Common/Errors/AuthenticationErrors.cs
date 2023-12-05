using FluentResults;
using MockInterview.Identity.Application.Common.Errors;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace MockInterview.Identity.Application.Authentication.Common.Errors;

public static class AuthenticationErrors
{
    public static readonly IError InvalidAccessToken =
        new AppError("Invalid access token", nameof(InvalidAccessToken), ErrorType.Validation);

    public static readonly IError RefreshTokenExpired =
        new AppError("Refresh token expired", nameof(RefreshTokenExpired), ErrorType.Validation);

    public static readonly IError UserWithSuchEmailAlreadyExists =
        new AppError("User with such email already exists", nameof(UserWithSuchEmailAlreadyExists),
            ErrorType.Conflict);

    public static readonly IError IncorrectEmailOrPassword =
        new AppError("Incorrect email or password", nameof(IncorrectEmailOrPassword), ErrorType.Validation);
}