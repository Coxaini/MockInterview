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
        Event(() => InterviewOrderPersisted, x => x.CorrelateById(m => m.Message.InterviewOrderId));
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
                .Publish(x => new PersistInterviewOrder(x.Saga.CorrelationId,
                    x.Saga.InitiatorCandidateId,
                    x.Saga.StartDateTime,
                    x.Saga.ProgrammingLanguage,
                    x.Saga.Technologies))
                .TransitionTo(Submitted));

        During(Submitted,
            When(InterviewOrderPersisted)
                .Publish(x => new FindMatch(x.Saga.CorrelationId))
                .TransitionTo(Persisted));

        During(Persisted,
            When(MatchFoundEvent)
                .Then(context =>
                {
                    context.Saga.MatchedCandidateId = context.Message.MatchCandidateId;
                    context.Saga.MatchedInterviewOrderId = context.Message.MatchInterviewOrderId;
                    context.Saga.MutualTechnologies = context.Message.MutualTechnologies;
                })
                .Publish(x =>
                    new ArrangeInterview(x.Saga.CorrelationId,
                        x.Saga.MatchedInterviewOrderId,
                        x.Saga.InitiatorCandidateId,
                        x.Saga.MatchedCandidateId,
                        x.Saga.StartDateTime,
                        x.Saga.ProgrammingLanguage,
                        x.Saga.MutualTechnologies))
                .TransitionTo(MatchFound));

        During(Persisted,
            When(MatchNotFoundEvent)
                .Then(x => _logger.LogInformation("Match not found for {InterviewOrderId}", x.Message.InterviewOrderId))
                .Finalize());

        During(Persisted,
            When(InterviewOrderNotFoundEvent)
                .Then(x => _logger.LogWarning("Interview order not found for {InterviewOrderId}",
                    x.Message.InterviewOrderId))
                .Finalize());

        During(MatchFound,
            When(InterviewArrangedEvent)
                .Then(x => _logger.LogInformation("Interview arranged for {InterviewOrderId}",
                    x.Message.InterviewOrderId))
                .Then(x => x.Saga.InterviewId = x.Message.InterviewId)
                .Publish(x =>
                    new CloseInterviewOrderPair(x.Saga.CorrelationId, x.Saga.MatchedInterviewOrderId))
                .TransitionTo(InterviewArranged));

        During(InterviewArranged,
            When(InterviewOrderPairDeletedEvent)
                .Then(x => _logger.LogInformation(
                    "Interview order pair deleted for {InterviewOrderId} and {MatchInterviewOrderId} order",
                    x.Saga.CorrelationId, x.Saga.MatchedInterviewOrderId))
                .Finalize());

        SetCompletedWhenFinalized();
    }

    public State Submitted { get; private set; }
    public State Persisted { get; private set; }
    public State MatchFound { get; private set; }
    public State InterviewArranged { get; private set; }

    public Event<InterviewOrderSubmitted> InterviewOrderSubmitted { get; private set; }
    public Event<InterviewOrderPersisted> InterviewOrderPersisted { get; private set; }
    public Event<MatchFound> MatchFoundEvent { get; private set; }
    public Event<MatchNotFound> MatchNotFoundEvent { get; private set; }
    public Event<InterviewOrderNotFound> InterviewOrderNotFoundEvent { get; private set; }
    public Event<InterviewArranged> InterviewArrangedEvent { get; private set; }
    public Event<InterviewOrderPairDeleted> InterviewOrderPairDeletedEvent { get; private set; }
}