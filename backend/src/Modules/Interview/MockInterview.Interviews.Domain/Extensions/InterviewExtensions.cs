using MockInterview.Interviews.Domain.Entities;
using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.Domain.Extensions;

public static class InterviewExtensions
{
    public static bool CanModifyQuestions(this Interview interview)
    {
        return interview.Status is not (InterviewStatus.Finished or InterviewStatus.Canceled);
    }
}