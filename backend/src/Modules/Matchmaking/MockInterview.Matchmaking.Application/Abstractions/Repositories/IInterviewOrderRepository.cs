using MockInterview.Matchmaking.Domain.Models.Interviews;

namespace MockInterview.Matchmaking.Application.Abstractions.Repositories;

public interface IInterviewOrderRepository
{
    Task AddInterviewOrderAsync(InterviewOrder interview);
    Task<InterviewOrder?> GetInterviewOrderByIdAsync(Guid id);
    Task<IList<InterviewOrder>> GetInterviewOrdersByUserIdAsync(Guid userId);
    Task<IList<InterviewOrder>> GetInterviewOrdersByStartDateTimeAsync(DateTime startsAt);
    Task<MatchedInterviewOrder?> GetBestMatchByMutualTechnologiesAsync(InterviewOrder matchOrder);
    Task<IList<InterviewOrder>> GetInterviewOrdersAtDateTimeAsync(DateTime dateTime);
    Task DeleteMatchInterviewOrdersAsync(Guid firstOrderId, Guid secondOrderId);
    Task DeleteInterviewOrderByIdAsync(Guid id);
}