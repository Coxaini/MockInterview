using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Common.Errors;
using MockInterview.Interviews.Application.Interviews.Models;
using MockInterview.Interviews.Application.Questions.Errors;
using MockInterview.Interviews.DataAccess;

namespace MockInterview.Interviews.Application.Interviews.Commands;

public class
    SubmitInterviewFeedbackCommandHandler : IRequestHandler<SubmitInterviewFeedbackCommand,
    Result<InterviewFeedbackDto>>
{
    private readonly InterviewsDbContext _dbContext;

    public SubmitInterviewFeedbackCommandHandler(InterviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<InterviewFeedbackDto>> Handle(SubmitInterviewFeedbackCommand request,
        CancellationToken cancellationToken)
    {
        var interviewQuestionList = await _dbContext.InterviewQuestionsLists
            .FirstOrDefaultAsync(l => l.AuthorId == request.UserId
                                      && l.InterviewId == request.InterviewId,
                cancellationToken);

        if (interviewQuestionList is null) return Result.Fail(QuestionListErrors.QuestionListNotFound);

        interviewQuestionList.SetFeedback(request.Feedback);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new InterviewFeedbackDto(request.InterviewId, request.UserId, request.Feedback);
    }
}