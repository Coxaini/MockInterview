using FluentResults;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace MockInterview.Interviews.Application.Conferences.Errors;

public class ConferenceErrors
{
    public static readonly IError FailedToCreateConference =
        new AppError("Failed to create conference due to concurrency problem", nameof(FailedToCreateConference),
            ErrorType.DatabaseError);

    public static readonly IError ConferenceNotFound = new AppError("Conference not found", nameof(ConferenceNotFound),
        ErrorType.NotFound);

    public static readonly IError UserIsNotInterviewer = new AppError("User is not interviewer",
        nameof(UserIsNotInterviewer),
        ErrorType.AccessDenied);
}