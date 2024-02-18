using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Questions.Errors;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Entities;
using MockInterview.Interviews.Domain.Extensions;

namespace MockInterview.Interviews.Application.Questions.Commands;

public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, Result<InterviewQuestionDto>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public AddQuestionCommandHandler(InterviewsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<InterviewQuestionDto>> Handle(AddQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var questionList = await _dbContext.InterviewQuestionsLists
            .Include(l => l.Interview)
            .FirstOrDefaultAsync(l => l.Id == request.QuestionListId
                                      && l.AuthorId == request.UserId,
                cancellationToken);

        if (questionList is null) return Result.Fail(QuestionListErrors.QuestionListNotFound);

        if (questionList.Interview?.CanModifyQuestions() == false)
            return Result.Fail(QuestionErrors.CannotModifyQuestionList);

        int maxOrderIndex = await _dbContext.InterviewQuestionsLists
            .Where(l => l.Id == request.QuestionListId)
            .SelectMany(l => l.Questions)
            .MaxAsync(q => (int?)q.OrderIndex, cancellationToken) ?? -1;

        var question = InterviewQuestion.Create(questionList, request.Text, request.DifficultyLevel,
            request.Tag,
            maxOrderIndex + 1);

        _dbContext.InterviewQuestions.Add(question);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok(_mapper.Map<InterviewQuestionDto>(question));
    }
}