using Microsoft.Extensions.Options;
using MockInterview.Interviews.Application.Interviews.BackgroundJobs;
using Quartz;

namespace MockInterview.Interviews.Infrastructure.BackgroundJobSetups;

public class InterviewTimeOutJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        options
            .AddJob<InterviewTimeOutJob>(builder => builder.WithIdentity(InterviewTimeOutJob.JobKey).StoreDurably());
    }
}