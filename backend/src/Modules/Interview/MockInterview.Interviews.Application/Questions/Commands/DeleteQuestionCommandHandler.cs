using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Questions.Errors;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Extensions;

namespace MockInterview.Interviews.Application.Questions.Commands;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Result>
{
    private readonly InterviewsDbContext _dbContext;

    public DeleteQuestionCommandHandler(InterviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _dbContext.InterviewQuestions
            .Include(q => q.InterviewQuestionsList)
            .ThenInclude(l => l.Interview)
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId
                                      && q.InterviewQuestionsListId == request.QuestionListId
                                      && q.InterviewQuestionsList.AuthorId == request.UserId,
                cancellationToken);

        if (question is null)
            return Result.Fail(QuestionErrors.QuestionNotFound);

        if (question.InterviewQuestionsList.Interview?.CanModifyQuestions() == false)
            return Result.Fail(QuestionErrors.CannotModifyQuestion);

        _dbContext.InterviewQuestions.Remove(question);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}