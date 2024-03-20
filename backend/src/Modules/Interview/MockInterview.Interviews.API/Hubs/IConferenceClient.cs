using MockInterview.Interviews.API.Requests;
using MockInterview.Interviews.Application.Conferences.Models;
using MockInterview.Interviews.Domain.Enumerations;

namespace MockInterview.Interviews.API.Hubs;

public interface IConferenceClient
{
    Task UserJoinedConference(Guid interviewId, Guid userId);
    Task UserLeftConference(Guid interviewId, Guid userId);
    Task ReceiveOffer(string offer);
    Task ReceiveAnswer(string answer);
    Task ReceiveIceCandidate(string iceCandidate);
    Task RoleSwapped(SwapRoleRequest request);
    Task QuestionChanged(Guid conferenceId, ConferenceQuestionDto currentQuestion);
}