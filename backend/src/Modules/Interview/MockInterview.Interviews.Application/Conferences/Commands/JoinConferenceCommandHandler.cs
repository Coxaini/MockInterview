using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Common.Errors;
using MockInterview.Interviews.Application.Common.Exceptions;
using MockInterview.Interviews.Application.Conferences.Models;
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

    public JoinConferenceCommandHandler(InterviewsDbContext dbContext, IRedisConnectionProvider connectionProvider)
    {
        _dbContext = dbContext;
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
                .FirstOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);

            if (interview is null)
                return Result.Fail(InterviewErrors.InterviewNotFound);

            var member = interview.Members.FirstOrDefault(m => m.UserId == request.UserId);

            if (member is null)
                return Result.Fail(InterviewErrors.InterviewIsNotBelongToUser);

            var result = await CreateSessionFromInterview(interview, request.UserId);

            if (result.IsFailed) return Result.Fail(result.Errors);

            session = result.Value;

            var peer = session.Members.First(m => m.Id != request.UserId);

            return new UserConferenceDto(
                request.InterviewId,
                ShouldUserSendOffer(session, request.UserId),
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
                peer.Id,
                peer.IsConnected);
        }
    }

    private bool ShouldUserSendOffer(ConferenceSession session, Guid userId)
    {
        return session.Members[0].Id == userId;
    }

    private async Task<Result<ConferenceSession>> CreateSessionFromInterview(Interview interview, Guid userId)
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
        int interviewerIndex = random.Next(interview.Members.Count);

        var members = interview.Members.Select((m, i) => new ConferenceUser
        {
            Id = m.UserId,
            IsConnected = m.UserId == userId,
            Role = i == interviewerIndex
                ? ConferenceMemberRole.Interviewer
                : ConferenceMemberRole.Interviewee
        });

        return new ConferenceSession
        {
            Id = interview.Id,
            Members = members.ToArray()
        };
    }
}