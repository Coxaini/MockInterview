using Microsoft.Extensions.Logging;
using MockInterview.Interviews.Contracts.Events;
using MockInterview.Interviews.DataAccess;
using Quartz;
using Shared.Messaging;

namespace MockInterview.Interviews.Application.Interviews.BackgroundJobs;

public class InterviewTimeOutJob : IJob
{
    public static readonly JobKey JobKey = new(nameof(InterviewTimeOutJob));
    public static readonly string InterviewIdKey = "interviewId";
    public static readonly string UserIdKey = "userId";

    private readonly ILogger<InterviewTimeOutJob> _logger;
    private readonly InterviewsDbContext _dbContext;
    private readonly IEventBus _eventBus;

    public InterviewTimeOutJob(ILogger<InterviewTimeOutJob> logger, InterviewsDbContext dbContext, IEventBus eventBus)
    {
        _logger = logger;
        _dbContext = dbContext;
        _eventBus = eventBus;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var interviewId = context.MergedJobDataMap.GetGuid(InterviewIdKey);
        var userId = context.MergedJobDataMap.GetGuid(UserIdKey);

        var interview = await _dbContext.Interviews.FindAsync(interviewId);

        if (interview is null)
        {
            _logger.LogWarning("Interview {InterviewId} not found", interviewId);
            return;
        }

        var now = DateTime.UtcNow;

        interview.EndInterview(now);

        await _dbContext.SaveChangesAsync();

        await _eventBus.PublishAsync(new InterviewEnded(interview.Id, userId, now));

        _logger.LogInformation("Interview time out job executed for interview: {Interview}", interview);
    }
}