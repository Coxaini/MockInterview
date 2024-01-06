namespace MockInterview.Interviews.Contracts.Commands;

public record ArrangeInterview(
    Guid InterviewOrderId,
    Guid FirstCandidateId,
    Guid SecondCandidateId,
    DateTime StartDateTime,
    string ProgrammingLanguage,
    IEnumerable<string> Technologies);