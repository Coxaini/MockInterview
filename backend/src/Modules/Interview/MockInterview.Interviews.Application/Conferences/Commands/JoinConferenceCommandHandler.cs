using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Common.Errors;
using MockInterview.Interviews.Application.Common.Exceptions;
using MockInterview.Interviews.Application.Conferences.Models;
using MockInterview.Interviews.Application.Questions.Models;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Entities;
using MockInterview.Interviews.Domain.Enumerations;
using MockInterview.Interviews.Domain.Models;
using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace MockInterview.Interviews.Application.Conferences.Commands;

public class JoinConferenceCommandHandler : IRequestHandler<JoinConferenceCommand, Result<UserConferenceDto>>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IRedisCollection<ConferenceSession> _conferenceSessionCollection;
    private readonly IMapper _mapper;

    public JoinConferenceCommandHandler(InterviewsDbContext dbContext, IRedisConnectionProvider connectionProvider,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _conferenceSessionCollection = connectionProvider.RedisCollection<ConferenceSession>();
    }

    public async Task<Result<UserConferenceDto>> Handle(JoinConferenceCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _conferenceSessionCollection
            .FirstOrDefaultAsync(s => s.Id == request.InterviewId);

        if (session is null)
        {
            var interview = await _dbContext.Interviews
                .AsNoTracking()
                .Include(i => i.Members.OrderBy(m => m.UserId))
                .Include(i => i.QuestionsLists)
                .ThenInclude(l => l.Questions.OrderBy(q => q.OrderIndex).Take(1))
                .FirstOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);

            if (interview is null)
                return Result.Fail(InterviewErrors.InterviewNotFound);

            var member = interview.Members.FirstOrDefault(m => m.UserId == request.UserId);

            if (member is null)
                return Result.Fail(InterviewErrors.InterviewIsNotBelongToUser);

            var result = await InsertSessionFromInterview(interview, request.UserId);

            if (result.IsFailed) return Result.Fail(result.Errors);

            session = result.Value;

            var peer = session.Members.First(m => m.Id != request.UserId);
            var user = session.Members.First(m => m.Id == request.UserId);

            return new UserConferenceDto(
                request.InterviewId,
                ShouldUserSendOffer(session, request.UserId),
                user.Role,
                GetFirstInterviewerQuestion(session),
                peer.Id,
                false);
        }
        else
        {
            var user = session.Members.FirstOrDefault(m => m.Id == request.UserId);

            if (user is null)
                return Result.Fail(InterviewErrors.InterviewIsNotBelongToUser);

            user.IsConnected = true;

            await _conferenceSessionCollection.SaveAsync();

            var peer = session.Members.First(m => m.Id != request.UserId);

            return new UserConferenceDto(
                request.InterviewId,
                ShouldUserSendOffer(session, request.UserId),
                user.Role,
                GetFirstInterviewerQuestion(session),
                peer.Id,
                peer.IsConnected);
        }
    }

    private ConferenceQuestionDto? GetFirstInterviewerQuestion(ConferenceSession session)
    {
        var interviewer = session.Members.First(m => m.Role == ConferenceMemberRole.Interviewer);
        return interviewer.CurrentQuestion is not null
            ? _mapper.Map<ConferenceQuestionDto>(interviewer.CurrentQuestion)
            : null;
    }

    private bool ShouldUserSendOffer(ConferenceSession session, Guid userId)
    {
        return session.Members[0].Id == userId;
    }

    private async Task<Result<ConferenceSession>> InsertSessionFromInterview(Interview interview, Guid userId)
    {
        var newSession = MapInterviewToSession(interview, userId);

        string? sessionId = await _conferenceSessionCollection.InsertAsync(newSession, WhenKey.NotExists);

        if (sessionId is null)
            throw new ConcurrencyException("Session already exists");

        return newSession;
    }

    private ConferenceSession MapInterviewToSession(Interview interview, Guid userId)
    {
        var random = new Random();
        var interviewerId = interview.Members[random.Next(interview.Members.Count)].UserId;

        var members = interview.Members.Select(m =>
        {
            var firstInterviewerQuestion = interview.GetQuestionsListByUserId(m.UserId).Questions.FirstOrDefault();
            return new ConferenceUser
            {
                Id = m.UserId,
                IsConnected = m.UserId == userId,
                CurrentQuestion = firstInterviewerQuestion is not null
                    ? _mapper.Map<ConferenceQuestion>(firstInterviewerQuestion)
                    : null,
                Role = m.UserId == interviewerId
                    ? ConferenceMemberRole.Interviewer
                    : ConferenceMemberRole.Interviewee
            };
        });

        return new ConferenceSession
        {
            Id = interview.Id,
            Members = members.ToArray()
        };
    }
}