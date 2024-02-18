namespace MockInterview.Matchmaking.Contracts.Events;

public record InterviewOrderCanceled(Guid InterviewOrderId, Guid CandidateId, DateTime CanceledAt);