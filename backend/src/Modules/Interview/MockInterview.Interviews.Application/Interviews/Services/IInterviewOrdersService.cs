using MockInterview.Interviews.Application.Interviews.Models;

namespace MockInterview.Interviews.Application.Interviews.Services;

public interface IInterviewOrdersService
{
    Task<InterviewOrderDto?> GetInterviewOrderAsync(Guid id, CancellationToken cancellationToken = default);
}