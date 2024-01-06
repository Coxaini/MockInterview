using Microsoft.Extensions.Options;
using MockInterview.Matchmaking.Application.Matching.BackgroundJobs;
using Quartz;

namespace MockInterview.Matchmaking.Infrastructure.BackgroundJobSetups;

public class PeerMatchingJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = new JobKey(nameof(PeerMatchingJob));
        options
            .AddJob<PeerMatchingJob>(builder => builder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger.ForJob(jobKey).WithCronSchedule("0 0 * ? * * *"));
    }
}