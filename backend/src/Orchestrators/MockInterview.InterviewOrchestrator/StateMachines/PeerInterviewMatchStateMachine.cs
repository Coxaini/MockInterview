using MassTransit;
using MockInterview.Interviews.Contracts.Commands;
using MockInterview.Interviews.Contracts.Events;
using MockInterview.Matchmaking.Contracts.Commands;
using MockInterview.Matchmaking.Contracts.Events;

namespace MockInterview.InterviewOrchestrator.StateMachines;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

// ReSharper disable UnusedAutoPropertyAccessor.Local
public class PeerInterviewMatchStateMachine : MassTransitStateMachine<PeerInterviewMatchState>
{
    public PeerInterviewMatchStateMachine()
    {
        Event(() => MatchFoundEvent, x => x.CorrelateById(m => m.Message.FirstInterviewOrderId));
        Event(() => InterviewArrangedEvent, x => x.CorrelateById(m => m.Message.InterviewOrderId));
        Event(() => InterviewOrderPairDeletedEvent, x => x.CorrelateById(m => m.Message.InitiatorOrderId));

        InstanceState(x => x.CurrentState);

        Initially(
            When(MatchFoundEvent)
                .Then(context =>
                {
                    context.Saga.FirstCandidateId = context.Message.FirstCandidateId;
                    context.Saga.SecondCandidateId = context.Message.SecondCandidateId;
                    context.Saga.FirstInterviewOrderId = context.Message.FirstInterviewOrderId;
                    context.Saga.SecondInterviewOrderId = context.Message.SecondInterviewOrderId;
                    context.Saga.StartDateTime = context.Message.StartDateTime;
                    context.Saga.ProgrammingLanguage = context.Message.ProgrammingLanguage;
                    context.Saga.MutualTechnologies = context.Message.MutualTechnologies;
                })
                .Publish(x =>
                    new ArrangeInterview(x.Saga.FirstInterviewOrderId,
                        x.Saga.SecondInterviewOrderId,
                        x.Saga.FirstCandidateId,
                        x.Saga.SecondCandidateId,
                        x.Saga.StartDateTime,
                        x.Saga.ProgrammingLanguage,
                        x.Saga.MutualTechnologies))
                .TransitionTo(PeerMatchFound));

        During(PeerMatchFound,
            When(InterviewArrangedEvent)
                .Then(context => context.Saga.InterviewId = context.Message.InterviewId)
                .Publish(x =>
                    new CloseInterviewOrderPair(x.Saga.FirstInterviewOrderId, x.Saga.SecondInterviewOrderId))
                .TransitionTo(InterviewArranged));

        During(InterviewArranged,
            When(InterviewOrderPairDeletedEvent)
                .Finalize());

        SetCompletedWhenFinalized();
    }

    public State PeerMatchFound { get; private set; }
    public State InterviewArranged { get; private set; }

    public Event<PeerMatchFound> MatchFoundEvent { get; private set; }
    public Event<InterviewArranged> InterviewArrangedEvent { get; private set; }
    public Event<InterviewOrderPairDeleted> InterviewOrderPairDeletedEvent { get; private set; }
}