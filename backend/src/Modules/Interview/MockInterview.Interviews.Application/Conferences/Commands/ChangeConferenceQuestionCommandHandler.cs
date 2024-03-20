using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Common.Errors;
using MockInterview.Interviews.Application.Conferences.Errors;
using MockInterview.Interviews.Application.Conferences.Models;
using MockInterview.Interviews.Application.Questions.Errors;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Enumerations;
using MockInterview.Interviews.Domain.Models;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace MockInterview.Interviews.Application.Conferences.Commands;

public class
    ChangeConferenceQuestionCommandHandler : IRequestHandler<ChangeConferenceQuestionCommand,
    Result<ConferenceSessionDto>>
{
    private readonly IRedisCollection<ConferenceSession> _conferenceSessionCollection;
    private readonly InterviewsDbContext _dbContext;
    private readonly IMapper _mapper;

    public ChangeConferenceQuestionCommandHandler(IRedisConnectionProvider connectionProvider,
        IMapper mapper, InterviewsDbContext dbContext)
    {
        _conferenceSessionCollection = connectionProvider.RedisCollection<ConferenceSession>();
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Result<ConferenceSessionDto>> Handle(ChangeConferenceQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _conferenceSessionCollection.FirstOrDefaultAsync(s => s.Id == request.ConferenceId);

        if (session is null)
            return Result.Fail(ConferenceErrors.ConferenceNotFound);

        var user = session.Members.First(m => m.Id == request.UserId);

        if (user.Role != ConferenceMemberRole.Interviewer)
            return Result.Fail(ConferenceErrors.UserIsNotInterviewer);

        var question = await _dbContext.InterviewQuestions
            .FirstOrDefaultAsync(
                i => i.Id == request.QuestionId && i.InterviewQuestionsList.InterviewId == request.ConferenceId,
                cancellationToken);

        if (question is null)
            return Result.Fail(QuestionErrors.QuestionNotFound);

        user.CurrentQuestion = _mapper.Map<ConferenceQuestion>(question);

        await _conferenceSessionCollection.SaveAsync();

        return _mapper.Map<ConferenceSessionDto>(session);
    }
}