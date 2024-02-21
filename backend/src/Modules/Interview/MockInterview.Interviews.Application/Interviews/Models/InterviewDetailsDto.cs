using MockInterview.Interviews.Application.Models;
using MockInterview.Interviews.Application.Questions.Models;

namespace MockInterview.Interviews.Application.Interviews.Models;

public record InterviewDetailsDto(
    Guid Id,
    UserDto? Mate,
    InterviewQuestionsListDto UserQuestionsList,
    InterviewQuestionsListDto? MateQuestionsList,
    DateTime StartDateTime,
    DateTime? EndDateTime,
    string ProgrammingLanguage,
    IEnumerable<string> Tags
);