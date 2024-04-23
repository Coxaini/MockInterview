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

    public static readonly IError InterviewIsFinished =
        new AppError("Interview is finished", nameof(InterviewIsFinished), ErrorType.Validation);

    public static readonly IError InterviewIsNotStarted =
        new AppError("Interview is not started", nameof(InterviewIsNotStarted), ErrorType.Validation);
}