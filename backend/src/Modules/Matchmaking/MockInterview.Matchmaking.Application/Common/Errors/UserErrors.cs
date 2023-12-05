using FluentResults;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace MockInterview.Matchmaking.Application.Common.Errors;

public static class UserErrors
{
    public static readonly IError UserNotFound =
        new AppError("User not found", nameof(UserNotFound), ErrorType.NotFound);
}