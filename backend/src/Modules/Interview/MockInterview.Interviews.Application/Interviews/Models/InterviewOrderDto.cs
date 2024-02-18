namespace MockInterview.Interviews.Application.Interviews.Models;

public record InterviewOrderDto(
    Guid Id,
    Guid CandidateId,
    DateTime StartDateTime,
    string ProgrammingLanguage,
    IEnumerable<string> Technologies);