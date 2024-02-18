using FluentResults;
using MediatR;
using MockInterview.Interviews.Application.Questions.Models;

namespace MockInterview.Interviews.Application.Questions.Queries;

public record GetQuestionsListQuery(Guid UserId, Guid QuestionsListId) : IRequest<Result<InterviewQuestionsListDto>>;