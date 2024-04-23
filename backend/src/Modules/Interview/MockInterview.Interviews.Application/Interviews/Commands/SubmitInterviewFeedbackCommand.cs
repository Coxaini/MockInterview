using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Interviews.Models;

namespace MockInterview.Interviews.Application.Interviews.Commands;

public record SubmitInterviewFeedbackCommand(Guid UserId, Guid InterviewId, string Feedback)
    : IRequest<Result<InterviewFeedbackDto>>;