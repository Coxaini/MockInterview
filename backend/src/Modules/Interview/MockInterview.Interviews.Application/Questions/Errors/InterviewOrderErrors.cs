using FluentResults;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace MockInterview.Interviews.Application.Questions.Errors;

public static class InterviewOrderErrors
{
    public static readonly IError NoAccessToOrder = new AppError(
        "You have no access to this order.", nameof(NoAccessToOrder), ErrorType.AccessDenied);

    public static readonly IError OrderNotFound =
        new AppError("Order not found.", nameof(OrderNotFound), ErrorType.NotFound);
}