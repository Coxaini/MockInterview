namespace MockInterview.Interviews.Contracts.Events;

public record InterviewCanceled(
    Guid InterviewId,
    Guid CancelerCandidateId,
    Guid AnotherCandidateId,
    Guid? AnotherCandidateOrderId,
    Guid? CancelerCandidateOrderId,
    string? CancelReason = null);