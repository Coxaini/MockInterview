using FluentResults;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace MockInterview.Identity.Application.Common.Errors;

public static class UserErrors
{
    public static readonly IError UserNotFound =
        new AppError("User not found", nameof(UserNotFound), ErrorType.NotFound);

    public static readonly IError SkillsNotValid =
        new AppError("Skills not valid", nameof(SkillsNotValid), ErrorType.Validation);
}