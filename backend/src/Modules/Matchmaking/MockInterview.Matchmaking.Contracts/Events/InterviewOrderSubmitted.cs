namespace MockInterview.Matchmaking.Contracts.Events;

public record InterviewOrderSubmitted(
    Guid Id,
    Guid CandidateId,
    DateTime StartDateTime,
    string ProgrammingLanguage,
    IEnumerable<string> Technologies);