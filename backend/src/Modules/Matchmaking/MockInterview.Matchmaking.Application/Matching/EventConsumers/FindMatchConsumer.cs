using MassTransit;
using Microsoft.Extensions.Logging;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Contracts.Commands;
using MockInterview.Matchmaking.Contracts.Events;
using Shared.Messaging;

namespace MockInterview.Matchmaking.Application.Matching.EventConsumers;

public class FindMatchConsumer : IConsumer<FindMatch>
{
    private readonly IEventBus _eventBus;
    private readonly IInterviewOrderRepository _interviewOrderRepository;
    private readonly ILogger<FindMatchConsumer> _logger;

    public FindMatchConsumer(IInterviewOrderRepository interviewOrderRepository,
        ILogger<FindMatchConsumer> logger, IEventBus eventBus)
    {
        _interviewOrderRepository = interviewOrderRepository;
        _logger = logger;
        _eventBus = eventBus;
    }

    public async Task Consume(ConsumeContext<FindMatch> context)
    {
        var interviewOrdered = context.Message;
        var order = await _interviewOrderRepository.GetInterviewOrderByIdAsync(interviewOrdered.InterviewOrderId);

        if (order is null)
        {
            _logger.LogInformation("Interview order with id {InterviewOrderId} not found. Can't match.",
                interviewOrdered.InterviewOrderId);
            await _eventBus.PublishAsync(new InterviewOrderNotFound(interviewOrdered.InterviewOrderId));
            return;
        }

        var bestMatch = await _interviewOrderRepository.GetBestMatchByMutualTechnologiesAsync(order);

        if (bestMatch is null)
        {
            _logger.LogDebug("No best match found for interview order with id {InterviewOrderId}",
                interviewOrdered.InterviewOrderId);
            await _eventBus.PublishAsync(new MatchNotFound(interviewOrdered.InterviewOrderId));
            return;
        }

        await context.Publish(new MatchFound(order.Id, bestMatch.InterviewOrderId,
            order.CandidateId, bestMatch.CandidateId,
            bestMatch.StartDateTime, bestMatch.ProgrammingLanguage,
            bestMatch.MutualTechnologies));

        await _interviewOrderRepository.CloseMatchInterviewOrdersAsync(order.Id, bestMatch.InterviewOrderId);
    }
}