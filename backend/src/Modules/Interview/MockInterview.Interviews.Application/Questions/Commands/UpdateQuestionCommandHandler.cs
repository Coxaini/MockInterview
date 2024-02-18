using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Questions.Errors;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Extensions;

namespace MockInterview.Interviews.Application.Questions.Commands;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Result<InterviewQuestionDto>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateQuestionCommandHandler(InterviewsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<InterviewQuestionDto>> Handle(UpdateQuestionCommand request,
        CancellationToken cancellationToken)
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


        question.Update(request.Text, request.Tag, request.DifficultyLevel);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok(_mapper.Map<InterviewQuestionDto>(question));
    }
}