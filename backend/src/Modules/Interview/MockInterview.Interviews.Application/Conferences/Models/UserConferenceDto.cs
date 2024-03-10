namespace MockInterview.Interviews.Application.Conferences.Models;

public record UserConferenceDto(
    Guid Id,
    bool ShouldSendOffer,
    Guid PeerId,
    bool IsPeerJoined
);