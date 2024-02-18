using FluentResults;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace MockInterview.Interviews.Application.Common.Errors;

public static class InterviewErrors
{
    public static readonly IError InterviewNotFound =
        new AppError("Interview not found", nameof(InterviewNotFound), ErrorType.NotFound);

    public static readonly IError InterviewIsNotBelongToUser =
        new AppError("Interview is not belong to user", nameof(InterviewIsNotBelongToUser), ErrorType.AccessDenied);
}