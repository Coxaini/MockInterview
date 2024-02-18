using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Questions.Models;

namespace MockInterview.Interviews.Application.Questions.Commands;

public record CreateQuestionsListForOrderCommand(Guid AuthorId, Guid InterviewOrderId)
    : IRequest<Result<InterviewQuestionsListDto>>;