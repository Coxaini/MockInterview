using MassTransit;
using MockInterview.InterviewOrchestrator.StateMachines;
using Shared.Messaging.Settings;

namespace MockInterview.InterviewOrchestrator;

public class InterviewOrchestratorMassTransitConfiguration : IMassTransitConfiguration
{
    public void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
    {
        configurator.AddSagaStateMachine<InterviewOrderStateMachine, InterviewOrderState>()
            .InMemoryRepository();

        configurator.AddSagaStateMachine<PeerInterviewMatchStateMachine, PeerInterviewMatchState>()
            .InMemoryRepository();
    }
}