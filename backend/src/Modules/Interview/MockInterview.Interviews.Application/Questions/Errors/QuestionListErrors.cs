using FluentResults;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace MockInterview.Interviews.Application.Questions.Errors;

public static class QuestionListErrors
{
    public static readonly IError QuestionListNotFound =
        new AppError("Question list not found.", nameof(QuestionListNotFound), ErrorType.NotFound);
}