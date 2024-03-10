using MockInterview.Interviews.Domain.Models;

namespace MockInterview.Interviews.Application.Conferences.Abstractions;

public interface IConferenceSessionRepository
{
    Task AddSessionAsync(ConferenceSession conferenceSession);
    Task<ConferenceSession?> GetSessionByIdAsync(string sessionId);
    Task RemoveSessionAsync(string sessionId);
}