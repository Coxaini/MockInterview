using MockInterview.Matchmaking.Domain.Models.Interviews;

namespace MockInterview.Matchmaking.Application.Matching.Services;

public interface IMatchingService
{
    IEnumerable<(InterviewOrder, InterviewOrder)> GetBestMatches(IList<InterviewOrder> interviewOrders);
}