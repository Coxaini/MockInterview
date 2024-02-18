using MassTransit;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Contracts.Commands;
using MockInterview.Matchmaking.Contracts.Events;

namespace MockInterview.Matchmaking.Application.Matching.EventConsumers;

public class CloseInterviewOrderPairConsumer : IConsumer<CloseInterviewOrderPair>
{
    private readonly IInterviewOrderRepository _interviewOrderRepository;

    public CloseInterviewOrderPairConsumer(IInterviewOrderRepository interviewOrderRepository)
    {
        _interviewOrderRepository = interviewOrderRepository;
    }

    public async Task Consume(ConsumeContext<CloseInterviewOrderPair> context)
    {
        var command = context.Message;
        await _interviewOrderRepository.CloseMatchInterviewOrdersAsync(command.InitiatorOrderId, command.MatchOrderId);

        await context.Publish(new InterviewOrderPairDeleted(command.InitiatorOrderId, command.MatchOrderId));
    }
}