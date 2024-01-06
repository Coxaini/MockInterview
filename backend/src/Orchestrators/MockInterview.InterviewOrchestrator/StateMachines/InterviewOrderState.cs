using MassTransit;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace MockInterview.InterviewOrchestrator.StateMachines;

public class InterviewOrderState : SagaStateMachineInstance
{
    public string CurrentState { get; set; }
    public Guid InitiatorCandidateId { get; set; }
    public Guid MatchedCandidateId { get; set; }

    public DateTime OrderDate { get; set; }
    public DateTime StartDateTime { get; set; }
    public string ProgrammingLanguage { get; set; }
    public IEnumerable<string> Technologies { get; set; }

    public Guid MatchInterviewOrderId { get; set; }
    public Guid CorrelationId { get; set; }
}