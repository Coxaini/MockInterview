using MassTransit;

namespace MockInterview.InterviewOrchestrator.StateMachines;

public class PeerInterviewMatchState : SagaStateMachineInstance
{
    public string CurrentState { get; set; } = null!;
    public Guid FirstCandidateId { get; set; }
    public Guid SecondCandidateId { get; set; }

    public Guid FirstInterviewOrderId { get; set; }
    public Guid SecondInterviewOrderId { get; set; }

    public DateTime StartDateTime { get; set; }
    public string ProgrammingLanguage { get; set; } = null!;
    public IEnumerable<string> MutualTechnologies { get; set; } = null!;

    public Guid? InterviewId { get; set; }

    public Guid CorrelationId { get; set; }
}