namespace MockInterview.Interviews.Application.Interviews.Services;

public interface IInterviewScheduler
{
    Task ScheduleInterviewTimeOutAsync(Guid interviewId, Guid userId, DateTimeOffset endDateTime,
        CancellationToken cancellationToken = default);

    Task UnScheduleInterviewTimeOutAsync(Guid interviewId, CancellationToken cancellationToken = default);
}