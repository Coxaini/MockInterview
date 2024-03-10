namespace MockInterview.Interviews.API.Hubs;

public interface IConferenceClient
{
    Task UserJoinedConference(Guid interviewId, Guid userId);

    Task UserLeftConference(Guid interviewId, Guid userId);
    Task ReceiveOffer(string offer);
    Task ReceiveAnswer(string answer);
    Task ReceiveIceCandidate(string iceCandidate);
}