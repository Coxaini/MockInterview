using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Questions.Errors;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.DataAccess;

namespace MockInterview.Interviews.Application.Questions.Commands;

public class SubmitFeedbackCommandHandler : IRequestHandler<SubmitFeedbackCommand, Result<InterviewQuestionDto>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public SubmitFeedbackCommandHandler(InterviewsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<InterviewQuestionDto>> Handle(SubmitFeedbackCommand request,
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

        if (question.InterviewQuestionsList.Interview is null ||
            question.InterviewQuestionsList.Interview.StartTime > DateTime.UtcNow)
            return Result.Fail(QuestionErrors.CannotSubmitQuestionFeedback);

        question.SetFeedback(1, request.Feedback);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok(_mapper.Map<InterviewQuestionDto>(question));
    }
}