using MockInterview.Matchmaking.Domain.Models.Interviews;

namespace MockInterview.Matchmaking.Application.Abstractions.Repositories;

public interface IInterviewTimeSlotsRepository
{
    Task<IList<InterviewTimeSlot>> GetInterviewTimeSlotsAsync(DateTime from, DateTime to,
        int startHour, int endHour, string programmingLanguage);
}