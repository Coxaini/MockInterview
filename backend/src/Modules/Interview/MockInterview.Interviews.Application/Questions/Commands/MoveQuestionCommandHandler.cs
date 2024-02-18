using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Questions.Errors;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Extensions;

namespace MockInterview.Interviews.Application.Questions.Commands;

public class MoveQuestionCommandHandler : IRequestHandler<MoveQuestionCommand, Result>
{
    private readonly InterviewsDbContext _dbContext;

    public MoveQuestionCommandHandler(InterviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(MoveQuestionCommand request, CancellationToken cancellationToken)
    {
        var questionWithInterview = await _dbContext.InterviewQuestions
            .GroupJoin(_dbContext.Interviews,
                q => q.InterviewQuestionsList.InterviewId,
                i => i.Id,
                (q, i) => new { Question = q, Interview = i })
            .SelectMany(qi => qi.Interview.DefaultIfEmpty(),
                (qi, i) => new { qi.Question, Interview = i })
            .FirstOrDefaultAsync(
                q => q.Question.Id == request.QuestionId
                     && q.Question.InterviewQuestionsListId == request.QuestionsListId
                     && q.Question.InterviewQuestionsList.AuthorId == request.UserId,
                cancellationToken);

        if (questionWithInterview is null)
            return Result.Fail(QuestionErrors.QuestionNotFound);

        if (questionWithInterview.Interview?.CanModifyQuestions() == false)
            return Result.Fail(QuestionErrors.CannotModifyQuestion);

        var question = questionWithInterview.Question;

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        if (request.NewIndex > question.OrderIndex) //down
            await _dbContext.InterviewQuestions
                .Where(q => q.InterviewQuestionsListId == request.QuestionsListId
                            && q.OrderIndex > question.OrderIndex &&
                            q.OrderIndex <= request.NewIndex)
                .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(q => q.OrderIndex,
                            q => q.OrderIndex - 1),
                    cancellationToken);
        else if (request.NewIndex < question.OrderIndex) //up
            await _dbContext.InterviewQuestions
                .Where(q => q.InterviewQuestionsListId == request.QuestionsListId
                            && q.OrderIndex < question.OrderIndex &&
                            q.OrderIndex >= request.NewIndex)
                .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(q => q.OrderIndex,
                            q => q.OrderIndex + 1),
                    cancellationToken);

        question.SetOrderIndex(request.NewIndex);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}