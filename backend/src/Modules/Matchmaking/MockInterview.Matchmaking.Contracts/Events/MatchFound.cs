namespace MockInterview.Matchmaking.Contracts.Events;

public record MatchFound(
    Guid InitiatorInterviewOrderId,
    Guid MatchInterviewOrderId,
    Guid InitiatorCandidateId,
    Guid MatchCandidateId,
    DateTime StartDateTime,
    string ProgrammingLanguage,
    IEnumerable<string> MutualTechnologies);