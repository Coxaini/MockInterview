using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Interviews.Application.Common.Errors;
using MockInterview.Interviews.Contracts.Events;
using MockInterview.Interviews.DataAccess;
using Shared.Messaging;

namespace MockInterview.Interviews.Application.Interviews.Commands;

public class CancelInterviewCommandHandler : IRequestHandler<CancelInterviewCommand, Result>
{
    private readonly InterviewsDbContext _dbContext;
    private readonly IEventBus _eventBus;

    public CancelInterviewCommandHandler(InterviewsDbContext dbContext, IEventBus eventBus)
    {
        _dbContext = dbContext;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(CancelInterviewCommand request, CancellationToken cancellationToken)
    {
        var interview = await _dbContext.Interviews
            .Include(i => i.Members)
            .FirstOrDefaultAsync(i => i.Id == request.InterviewId, cancellationToken);

        if (interview is null) return Result.Fail(InterviewErrors.InterviewNotFound);

        if (!interview.IsMember(request.UserId))
            return Result.Fail(InterviewErrors.InterviewIsNotBelongToUser);

        _dbContext.Interviews.Remove(interview);

        var canceler = interview.Members.First(m => m.UserId == request.UserId);
        var anotherMember = interview.GetMateOfMember(request.UserId);

        var canceledEvent = new InterviewCanceled(interview.Id, canceler.UserId, anotherMember.UserId,
            anotherMember.InterviewOrderId, canceler.InterviewOrderId, request.CancelReason);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _eventBus.PublishAsync(canceledEvent, cancellationToken);

        return Result.Ok();
    }
}