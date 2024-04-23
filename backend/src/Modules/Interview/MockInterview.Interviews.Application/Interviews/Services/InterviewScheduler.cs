using Microsoft.Extensions.Logging;
using MockInterview.Interviews.Application.Interviews.BackgroundJobs;
using Quartz;

namespace MockInterview.Interviews.Application.Interviews.Services;

public class InterviewScheduler : IInterviewScheduler
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly ILogger<InterviewScheduler> _logger;

    public InterviewScheduler(ISchedulerFactory schedulerFactory, ILogger<InterviewScheduler> logger)
    {
        _schedulerFactory = schedulerFactory;
        _logger = logger;
    }

    public async Task ScheduleInterviewTimeOutAsync(Guid interviewId, Guid userId, DateTimeOffset endDateTime,
        CancellationToken cancellationToken = default)
    {
        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

        var trigger = TriggerBuilder.Create()
            .WithIdentity(interviewId.ToString())
            .ForJob(InterviewTimeOutJob.JobKey)
            .UsingJobData(InterviewTimeOutJob.InterviewIdKey, interviewId)
            .UsingJobData(InterviewTimeOutJob.UserIdKey, userId)
            .StartAt(endDateTime)
            .Build();

        await scheduler.ScheduleJob(trigger, cancellationToken);
    }

    public async Task UnScheduleInterviewTimeOutAsync(Guid interviewId, CancellationToken cancellationToken = default)
    {
        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

        var isUnScheduled = await scheduler.UnscheduleJob(new TriggerKey(interviewId.ToString()), cancellationToken);

        if (!isUnScheduled)
            _logger.LogWarning("Failed to unschedule interview timeout job for interview {InterviewId}", interviewId);
        else
            _logger.LogInformation("Interview timeout job for interview {InterviewId} unscheduled", interviewId);
    }
}