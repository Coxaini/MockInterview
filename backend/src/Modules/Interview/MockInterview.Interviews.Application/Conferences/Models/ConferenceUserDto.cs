using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.Application.Conferences.Models;

public record ConferenceUserDto(
    Guid Id,
    ConferenceMemberRole Role,
    ConferenceQuestionDto? CurrentQuestion,
    bool IsConnected
);