using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Common.Errors;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.Application.Questions.Commands;

public class CreateQuestionsListForInterviewCommandHandler : IRequestHandler<CreateQuestionsListForInterviewCommand,
    Result<InterviewQuestionsListDto>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateQuestionsListForInterviewCommandHandler(InterviewsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<InterviewQuestionsListDto>> Handle(CreateQuestionsListForInterviewCommand request,
        CancellationToken cancellationToken)
    {
        var questionsList =
            await _dbContext.InterviewQuestionsLists
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.InterviewId == request.InterviewId
                                          && l.AuthorId == request.AuthorId,
                    cancellationToken);

        if (questionsList is not null)
        {
            if (questionsList.AuthorId != request.AuthorId)
                return Result.Fail(InterviewErrors.InterviewIsNotBelongToUser);

            return _mapper.Map<InterviewQuestionsListDto>(questionsList);
        }

        var interview = await _dbContext.Interviews
            .AsNoTracking()
            .Include(i => i.Members)
            .FirstOrDefaultAsync(o => o.Id == request.InterviewId, cancellationToken);

        if (interview is null)
            return Result.Fail(InterviewErrors.InterviewNotFound);

        if (interview.Members.All(m => m.UserId != request.AuthorId))
            return Result.Fail(InterviewErrors.InterviewIsNotBelongToUser);

        var interviewOrderId = interview.Members
            .First(m => m.UserId == request.AuthorId).InterviewOrderId;

        var newQuestionsList = InterviewQuestionsList.Create(interview, request.AuthorId, interviewOrderId);

        _dbContext.InterviewQuestionsLists.Add(newQuestionsList);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<InterviewQuestionsListDto>(newQuestionsList);
    }
}