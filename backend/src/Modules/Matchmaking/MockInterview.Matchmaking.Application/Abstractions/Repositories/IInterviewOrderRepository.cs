using MockInterview.Matchmaking.Domain.Models.Interviews;

namespace MockInterview.Matchmaking.Application.Abstractions.Repositories;

public interface IInterviewOrderRepository
{
    Task AddInterviewOrderAsync(InterviewOrder interview);
}