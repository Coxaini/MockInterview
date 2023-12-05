using FluentResults;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace MockInterview.Matchmaking.Application.Skills.Errors;

public static class SkillsErrors
{
    public static readonly IError SkillsNotValid =
        new AppError("Skills not valid", nameof(SkillsNotValid), ErrorType.Validation);
}