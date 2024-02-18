using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Questions.Models;

namespace MockInterview.Interviews.Application.Questions.Commands;

public record CreateQuestionsListForInterviewCommand(Guid AuthorId, Guid InterviewId)
    : IRequest<Result<InterviewQuestionsListDto>>;