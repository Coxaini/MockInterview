namespace MockInterview.Matchmaking.Contracts.Events;

public record InterviewOrderPairDeleted(Guid InitiatorOrderId, Guid MatchOrderId);