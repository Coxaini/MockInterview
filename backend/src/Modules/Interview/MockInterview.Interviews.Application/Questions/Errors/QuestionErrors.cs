using FluentResults;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace MockInterview.Interviews.Application.Questions.Errors;

public static class QuestionErrors
{
    public static readonly IError QuestionNotFound = new AppError("Question not found.", nameof(QuestionNotFound),
        ErrorType.NotFound);

    public static readonly IError NoAccessToQuestion = new AppError("You have no access to this question.",
        nameof(NoAccessToQuestion), ErrorType.AccessDenied);

    public static readonly IError CannotModifyQuestion = new AppError(
        "Cannot modify question. Interview is in progress or finished.",
        nameof(CannotModifyQuestion), ErrorType.AccessDenied);

    public static readonly IError CannotModifyQuestionList = new AppError(
        "Cannot modify question list. Interview is in progress or finished.",
        nameof(CannotModifyQuestionList), ErrorType.AccessDenied);

    public static readonly IError CannotSubmitQuestionFeedback = new AppError(
        "Cannot submit question feedback. Interview is not started yet.",
        nameof(CannotSubmitQuestionFeedback), ErrorType.AccessDenied);
}