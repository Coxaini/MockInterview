using FluentResults;
using MediatR;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Application.Interviews.Errors;
using MockInterview.Matchmaking.Contracts.Events;
using Shared.Messaging;

namespace MockInterview.Matchmaking.Application.Interviews.Commands;

public class CancelInterviewOrderCommandHandler : IRequestHandler<CancelInterviewOrderCommand, Result>
{
    private readonly IEventBus _eventBus;
    private readonly IInterviewOrderRepository _interviewOrderRepository;

    public CancelInterviewOrderCommandHandler(IInterviewOrderRepository interviewOrderRepository, IEventBus eventBus)
    {
        _interviewOrderRepository = interviewOrderRepository;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(CancelInterviewOrderCommand request, CancellationToken cancellationToken)
    {
        var interviewOrder = await _interviewOrderRepository.GetInterviewOrderByIdAsync(request.InterviewOrderId);

        if (interviewOrder is null) return Result.Fail(InterviewOrderErrors.InterviewOrderNotFound);

        if (interviewOrder.CandidateId != request.UserId)
            return Result.Fail(InterviewOrderErrors.InterviewOrderDoesNotBelongToUser);

        await _interviewOrderRepository.DeleteInterviewOrderByIdAsync(request.InterviewOrderId);

        await _eventBus.PublishAsync(new InterviewOrderCanceled(request.InterviewOrderId, request.UserId,
            DateTime.UtcNow));

        return Result.Ok();
    }
}