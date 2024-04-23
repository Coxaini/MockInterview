using FluentResults;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MockInterview.Interviews.Application.Common.Exceptions;
using MockInterview.Interviews.Application.Conferences.Commands;
using MockInterview.Interviews.Application.Conferences.Errors;
using MockInterview.Interviews.Application.Conferences.Models;
using MockInterview.Interviews.Application.Conferences.PipelineBehaviors;
using MockInterview.Interviews.Application.Interviews.Services;
using MockInterview.Interviews.Domain.Models;
using Polly;
using Polly.Fallback;
using Polly.Retry;

namespace MockInterview.Interviews.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.AddBehavior<IPipelineBehavior<JoinConferenceCommand, Result<UserConferenceDto>>,
                JoinConferenceRetryBehavior<JoinConferenceCommand, Result<UserConferenceDto>>>();
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddResiliencePipelines();

        services.AddScoped<IInterviewOrdersService, InterviewOrdersService>();
        services.AddScoped<IInterviewScheduler, InterviewScheduler>();

        return services;
    }

    private static IServiceCollection AddResiliencePipelines(this IServiceCollection services)
    {
        services
            .AddResiliencePipeline<string, Result<UserConferenceDto>>("redis-conference-retry",
                pipelineBuilder =>
                {
                    pipelineBuilder.AddFallback(new FallbackStrategyOptions<Result<UserConferenceDto>>()
                    {
                        FallbackAction = _ =>
                            Outcome.FromResultAsValueTask(
                                Result.Fail<UserConferenceDto>(ConferenceErrors.FailedToCreateConference)),
                        ShouldHandle = new PredicateBuilder<Result<UserConferenceDto>>()
                            .Handle<ConcurrencyException>()
                    }).AddRetry(new RetryStrategyOptions<Result<UserConferenceDto>>()
                    {
                        MaxRetryAttempts = 2,
                        BackoffType = DelayBackoffType.Constant,
                        Delay = TimeSpan.FromMilliseconds(10),
                        ShouldHandle = new PredicateBuilder<Result<UserConferenceDto>>()
                            .Handle<ConcurrencyException>()
                    });
                });

        return services;
    }
}