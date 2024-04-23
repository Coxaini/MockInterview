using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Common.Errors;
using MockInterview.Interviews.Contracts.Events;
using MockInterview.Interviews.DataAccess;
using MockInterview.Interviews.Domain.Enumerations;
using Shared.Messaging;

namespace MockInterview.Interviews.Application.Interviews.Commands;

public class EndInterviewCommandHandler : IRequestHandler<EndInterviewCommand, Result>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IEventBus _eventBus;

    public EndInterviewCommandHandler(InterviewsDbContext dbContext, IEventBus eventBus)
    {
        _dbContext = dbContext;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(EndInterviewCommand request, CancellationToken cancellationToken)
    {
        var interview = await _dbContext.Interviews
            .Include(i => i.Members)
            .FirstOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);

        if (interview is null)
            return Result.Fail(InterviewErrors.InterviewNotFound);

        if (!interview.IsMember(request.UserId))
            return Result.Fail(InterviewErrors.InterviewIsNotBelongToUser);

        if (interview.Status != InterviewStatus.InProgress) return Result.Fail(InterviewErrors.InterviewIsNotStarted);

        var now = DateTime.UtcNow;

        interview.EndInterview(now);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _eventBus.PublishAsync(new InterviewEnded(interview.Id, request.UserId, now));

        return Result.Ok();
    }
}