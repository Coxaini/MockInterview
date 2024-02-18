namespace MockInterview.Matchmaking.Application.Interviews.Models;

public record InterviewOrderDto(
    Guid Id,
    string ProgrammingLanguage,
    IEnumerable<string> Tags,
    DateTime StartDateTime);