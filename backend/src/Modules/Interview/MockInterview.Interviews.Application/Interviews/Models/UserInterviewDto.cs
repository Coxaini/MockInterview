using MockInterview.Interviews.Application.Models;

namespace MockInterview.Interviews.Application.Interviews.Models;

public record UserInterviewDto(
    Guid Id,
    UserDto Mate,
    DateTime StartDateTime,
    DateTime? EndDateTime,
    string ProgrammingLanguage,
    IEnumerable<string> Tags
);