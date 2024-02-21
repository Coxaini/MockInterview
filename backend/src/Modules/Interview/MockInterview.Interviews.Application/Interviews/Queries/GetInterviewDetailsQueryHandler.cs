using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Common.Errors;
using MockInterview.Interviews.Application.Interviews.Models;
using MockInterview.Interviews.Application.Models;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.Application.Interviews.Queries;

public class
    GetInterviewDetailsQueryHandler : IRequestHandler<GetInterviewDetailsQuery, Result<InterviewDetailsDto>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetInterviewDetailsQueryHandler(InterviewsDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<InterviewDetailsDto>> Handle(GetInterviewDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var interview = await _dbContext.Interviews
            .AsNoTracking()
            .AsSplitQuery()
            .Include(i => i.Members)
            .ThenInclude(i => i.User)
            .Include(i => i.QuestionsLists)
            .ThenInclude(ql => ql.Questions.OrderBy(q => q.OrderIndex))
            .FirstOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);

        if (interview is null)
            return Result.Fail(InterviewErrors.InterviewNotFound);

        if (!interview.IsMember(request.UserId))
            return Result.Fail(InterviewErrors.InterviewIsNotBelongToUser);

        var mate = interview.GetMateOfMember(request.UserId);

        var interviewDetails = new InterviewDetailsDto(
            interview.Id,
            _mapper.Map<UserDto>(mate.User),
            _mapper.Map<InterviewQuestionsListDto>(interview.GetQuestionsListByUserId(request.UserId)),
            interview.EndTime != null ? MapMateQuestionsList(interview.GetQuestionsListByUserId(mate.UserId)) : null,
            interview.StartTime,
            interview.EndTime,
            interview.ProgrammingLanguage,
            interview.Tags
        );

        return interviewDetails;
    }

    private InterviewQuestionsListDto MapMateQuestionsList(InterviewQuestionsList questionsList)
    {
        return new InterviewQuestionsListDto(
            questionsList.Id,
            questionsList.InterviewOrderId,
            questionsList.InterviewId,
            questionsList.AuthorId,
            questionsList.Feedback,
            questionsList.Questions.Select(q => new InterviewQuestionDto(
                q.Id,
                q.Text,
                questionsList.IsFeedbackSent ? q.Feedback : null,
                questionsList.IsFeedbackSent ? q.Rating : null,
                q.Tag,
                q.DifficultyLevel,
                q.OrderIndex
            ))
        );
    }
}