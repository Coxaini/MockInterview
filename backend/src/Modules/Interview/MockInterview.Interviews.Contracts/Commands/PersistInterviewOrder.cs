namespace MockInterview.Interviews.Contracts.Commands;

public record PersistInterviewOrder(
    Guid InterviewOrderId,
    Guid CandidateId,
    DateTime StartDateTime,
    string ProgrammingLanguage,
    IEnumerable<string> Technologies);