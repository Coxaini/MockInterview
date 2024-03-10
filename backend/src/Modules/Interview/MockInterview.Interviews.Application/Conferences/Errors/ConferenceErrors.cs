using FluentResults;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace MockInterview.Interviews.Application.Conferences.Errors;

public class ConferenceErrors
{
    public static readonly IError FailedToCreateConference =
        new AppError("Failed to create conference due to concurrency problem", nameof(FailedToCreateConference),
            ErrorType.DatabaseError);
}