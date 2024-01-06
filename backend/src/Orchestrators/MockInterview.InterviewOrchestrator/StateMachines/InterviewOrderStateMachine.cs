using MassTransit;
using Microsoft.Extensions.Logging;
using MockInterview.Interviews.Contracts.Commands;
using MockInterview.Interviews.Contracts.Events;
using MockInterview.Matchmaking.Contracts.Commands;
using MockInterview.Matchmaking.Contracts.Events;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace MockInterview.InterviewOrchestrator.StateMachines;

public class InterviewOrderStateMachine : MassTransitStateMachine<InterviewOrderState>
{
    private readonly ILogger<InterviewOrderStateMachine> _logger;

    public InterviewOrderStateMachine(ILogger<InterviewOrderStateMachine> logger)
    {
        _logger = logger;

        Event(() => InterviewOrderSubmitted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => MatchFoundEvent, x => x.CorrelateById(m => m.Message.InitiatorInterviewOrderId));
        Event(() => MatchNotFoundEvent, x => x.CorrelateById(m => m.Message.InterviewOrderId));
        Event(() => InterviewOrderNotFoundEvent, x => x.CorrelateById(m => m.Message.InterviewOrderId));
        Event(() => InterviewArrangedEvent, x => x.CorrelateById(m => m.Message.InterviewOrderId));
        Event(() => InterviewOrderPairDeletedEvent, x => x.CorrelateById(m => m.Message.InitiatorOrderId));

        InstanceState(x => x.CurrentState);

        Initially(
            When(InterviewOrderSubmitted)
                .Then(context =>
                {
                    context.Saga.CorrelationId = context.Message.Id;
                    context.Saga.InitiatorCandidateId = context.Message.CandidateId;
                    context.Saga.StartDateTime = context.Message.StartDateTime;
                    context.Saga.ProgrammingLanguage = context.Message.ProgrammingLanguage;
                    context.Saga.Technologies = context.Message.Technologies;
                })
                .Publish(x => new FindMatch(x.Saga.CorrelationId))
                .TransitionTo(Submitted));

        During(Submitted,
            When(MatchFoundEvent)
                .Then(context =>
                {
                    context.Saga.MatchedCandidateId = context.Message.MatchCandidateId;
                    context.Saga.MatchInterviewOrderId = context.Message.MatchInterviewOrderId;
                })
                .Publish(x =>
                    new ArrangeInterview(x.Saga.CorrelationId,
                        x.Saga.InitiatorCandidateId,
                        x.Saga.MatchedCandidateId,
                        x.Saga.StartDateTime,
                        x.Saga.ProgrammingLanguage,
                        x.Saga.Technologies))
                .TransitionTo(MatchFound));

        During(Submitted,
            When(MatchNotFoundEvent)
                .Then(x => _logger.LogInformation("Match not found for {InterviewOrderId}", x.Message.InterviewOrderId))
                .Finalize());

        During(Submitted,
            When(InterviewOrderNotFoundEvent)
                .Then(x => _logger.LogWarning("Interview order not found for {InterviewOrderId}",
                    x.Message.InterviewOrderId))
                .Finalize());

        During(MatchFound,
            When(InterviewArrangedEvent)
                .Then(x => _logger.LogInformation("Interview arranged for {InterviewOrderId}",
                    x.Message.InterviewOrderId))
                .Publish(x =>
                    new CloseInterviewOrderPair(x.Saga.CorrelationId, x.Saga.MatchInterviewOrderId))
                .TransitionTo(InterviewArranged));

        During(InterviewArranged,
            When(InterviewOrderPairDeletedEvent)
                .Then(x => _logger.LogInformation(
                    "Interview order pair deleted for {InterviewOrderId} and {MatchInterviewOrderId} order",
                    x.Saga.CorrelationId, x.Saga.MatchInterviewOrderId))
                .Finalize());
    }

    public State Submitted { get; private set; }
    public State MatchFound { get; private set; }
    public State InterviewArranged { get; private set; }

    public Event<InterviewOrderSubmitted> InterviewOrderSubmitted { get; private set; }
    public Event<MatchFound> MatchFoundEvent { get; private set; }
    public Event<MatchNotFound> MatchNotFoundEvent { get; private set; }
    public Event<InterviewOrderNotFound> InterviewOrderNotFoundEvent { get; private set; }
    public Event<InterviewArranged> InterviewArrangedEvent { get; private set; }
    public Event<InterviewOrderPairDeleted> InterviewOrderPairDeletedEvent { get; private set; }
}