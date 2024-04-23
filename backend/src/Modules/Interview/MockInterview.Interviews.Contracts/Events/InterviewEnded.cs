namespace MockInterview.Interviews.Contracts.Events;

public record InterviewEnded(Guid InterviewId, Guid EndedByUserId, DateTime EndTime);