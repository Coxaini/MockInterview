using FluentResults;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace MockInterview.Interviews.Application.InterviewOrders.Errors;

public static class InterviewOrderErrors
{
    public static readonly IError InterviewOrderNotFound =
        new AppError("Interview order not found", nameof(InterviewOrderNotFound), ErrorType.NotFound);

    public static readonly IError InterviewOrderNotOwnedByUser =
        new AppError("Interview order not owned by user", nameof(InterviewOrderNotOwnedByUser), ErrorType.AccessDenied);
}