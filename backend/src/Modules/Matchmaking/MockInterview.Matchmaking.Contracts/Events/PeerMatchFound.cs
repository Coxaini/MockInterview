namespace MockInterview.Matchmaking.Contracts.Events;

public record PeerMatchFound(
    Guid FirstInterviewOrderId,
    Guid SecondInterviewOrderId,
    Guid FirstCandidateId,
    Guid SecondCandidateId,
    DateTime StartDateTime,
    string ProgrammingLanguage,
    IEnumerable<string> MutualTechnologies);