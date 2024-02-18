namespace MockInterview.Matchmaking.Contracts.Commands;

public record CloseInterviewOrderPair(Guid InitiatorOrderId, Guid MatchOrderId);