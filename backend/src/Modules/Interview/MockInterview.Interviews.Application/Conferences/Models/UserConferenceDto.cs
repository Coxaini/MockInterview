using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.Application.Conferences.Models;

public record UserConferenceDto(
    Guid Id,
    bool ShouldSendOffer,
    ConferenceMemberRole UserRole,
    ConferenceQuestionDto? CurrentQuestion,
    Guid PeerId,
    bool IsPeerJoined
);