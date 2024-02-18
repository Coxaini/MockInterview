namespace MockInterview.Matchmaking.Contracts.Events;

public record InterviewOrderProcessed(
    Guid InterviewOrderId,
    Guid CandidateId,
    DateTime StartDateTime,
    string ProgrammingLanguage,
    IEnumerable<string> Technologies,
    Guid? InterviewId = null);