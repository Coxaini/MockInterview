namespace MockInterview.Interviews.Contracts.Commands;

public record ArrangeInterview(
    Guid InterviewOrderId,
    Guid MatchedInterviewOrderId,
    Guid InitiatorCandidateId,
    Guid MatchedCandidateId,
    DateTime StartDateTime,
    string ProgrammingLanguage,
    IEnumerable<string> MutualTechnologies);