using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using MockInterview.Interviews.Application.Conferences.Commands;
using MockInterview.Interviews.Application.Conferences.Models;
using Polly;
using Polly.Registry;

namespace MockInterview.Interviews.Application.Conferences.PipelineBehaviors;

public class JoinConferenceRetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : JoinConferenceCommand where TResponse : Result<UserConferenceDto>
{
    private readonly ILogger<JoinConferenceRetryBehavior<TRequest, TResponse>> _logger;

    private readonly ResiliencePipeline<Result<UserConferenceDto>> _resiliencePipeline;

    public JoinConferenceRetryBehavior(ILogger<JoinConferenceRetryBehavior<TRequest, TResponse>> logger,
        ResiliencePipelineProvider<string> resiliencePipelineProvider)
    {
        _logger = logger;
        _resiliencePipeline =
            resiliencePipelineProvider.GetPipeline<Result<UserConferenceDto>>("redis-conference-retry");
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("JoinConferenceRetryBehavior");
        var result = await _resiliencePipeline.ExecuteAsync(async (_) => await next(), cancellationToken);

        return result;
    }
}