using FluentResults;
using MapsterMapper;
using MediatR;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Application.Interviews.Models;
using MockInterview.Matchmaking.Contracts.Events;
using MockInterview.Matchmaking.Domain.Models.Interviews;
using Shared.Messaging;

namespace MockInterview.Matchmaking.Application.Interviews.Commands;

public class OrderInterviewSlotCommandHandler : IRequestHandler<OrderInterviewSlotCommand, Result<InterviewOrderDto>>
{
    private readonly IEventBus _eventBus;
    private readonly IInterviewOrderRepository _interviewOrderRepository;
    private readonly IMapper _mapper;

    public OrderInterviewSlotCommandHandler(IInterviewOrderRepository interviewOrderRepository, IMapper mapper,
        IEventBus eventBus)
    {
        _interviewOrderRepository = interviewOrderRepository;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task<Result<InterviewOrderDto>> Handle(OrderInterviewSlotCommand request,
        CancellationToken cancellationToken)
    {
        var interviewOrder = new InterviewOrder(Guid.NewGuid(), request.UserId, request.InterviewDate,
            request.ProgrammingLanguage,
            request.Technologies);

        await _interviewOrderRepository.AddInterviewOrderAsync(interviewOrder);

        await _eventBus.PublishAsync(new InterviewOrderSubmitted(interviewOrder.Id, interviewOrder.CandidateId,
            interviewOrder.StartDateTime, interviewOrder.ProgrammingLanguage, interviewOrder.Technologies));

        return _mapper.Map<InterviewOrderDto>(interviewOrder);
    }
}