using MockInterview.Interviews.Application.Models;

namespace MockInterview.Interviews.Application.Interviews.Models;

public record InterviewDto(
    Guid Id,
    UserDto Mate,
    DateTime StartDateTime,
    DateTime? EndDateTime,
    string ProgrammingLanguage,
    IEnumerable<string> Tags
);