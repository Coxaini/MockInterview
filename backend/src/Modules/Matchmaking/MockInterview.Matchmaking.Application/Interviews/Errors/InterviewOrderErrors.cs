using FluentResults;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace MockInterview.Matchmaking.Application.Interviews.Errors;

public static class InterviewOrderErrors
{
    public static readonly IError InterviewOrderNotFound =
        new AppError("Interview order not found", nameof(InterviewOrderNotFound), ErrorType.NotFound);

    public static readonly IError InterviewOrderDoesNotBelongToUser =
        new AppError("Interview order does not belong to user", nameof(InterviewOrderDoesNotBelongToUser),
            ErrorType.AccessDenied);
}