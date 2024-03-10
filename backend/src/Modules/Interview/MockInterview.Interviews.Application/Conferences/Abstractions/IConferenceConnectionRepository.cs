using MockInterview.Interviews.Domain.Models;

namespace MockInterview.Interviews.Application.Conferences.Abstractions;

public interface IConferenceConnectionRepository
{
    Task AddConnectionAsync(ConferenceUser conferenceUser);
    Task<ConferenceUser?> GetConnectionByUserIdAsync(Guid userId);
    Task RemoveUserConnectionAsync(Guid userId);
}