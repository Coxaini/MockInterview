namespace MockInterview.Interviews.Application.Interviews.Models;

public record InterviewFeedbackDto(
    Guid InterviewId,
    Guid UserId,
    string Feedback
);