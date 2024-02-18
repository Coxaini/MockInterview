using MassTransit;
using MockInterview.Interviews.Contracts.Events;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Contracts.Events;
using Shared.Messaging;

namespace MockInterview.Matchmaking.Application.Interviews.EventConsumers;

public class InterviewCanceledHandler : IConsumer<InterviewCanceled>
{
    private readonly IEventBus _eventBus;
    private readonly IInterviewOrderRepository _interviewOrderRepository;

    public InterviewCanceledHandler(IInterviewOrderRepository interviewOrderRepository, IEventBus eventBus)
    {
        _interviewOrderRepository = interviewOrderRepository;
        _eventBus = eventBus;
    }

    public async Task Consume(ConsumeContext<InterviewCanceled> context)
    {
        if (context.Message.AnotherCandidateOrderId is not null)
            await _interviewOrderRepository.OpenInterviewOrderAsync(context.Message.AnotherCandidateOrderId.Value);

        if (context.Message.CancelerCandidateOrderId is not null)
            await _eventBus.PublishAsync(new InterviewOrderCanceled(context.Message.CancelerCandidateOrderId.Value,
                context.Message.CancelerCandidateId, DateTime.UtcNow));
    }
}