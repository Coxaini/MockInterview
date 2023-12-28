using FluentResults;
using MapsterMapper;
using MediatR;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Application.Interviews.Models;
using MockInterview.Matchmaking.Domain.Models.Interviews;

namespace MockInterview.Matchmaking.Application.Interviews.Commands;

public class OrderInterviewSlotCommandHandler : IRequestHandler<OrderInterviewSlotCommand, Result<InterviewOrderDto>>
{
    private readonly IInterviewOrderRepository _interviewOrderRepository;
    private readonly IMapper _mapper;

    public OrderInterviewSlotCommandHandler(IInterviewOrderRepository interviewOrderRepository, IMapper mapper)
    {
        _interviewOrderRepository = interviewOrderRepository;
        _mapper = mapper;
    }

    public async Task<Result<InterviewOrderDto>> Handle(OrderInterviewSlotCommand request,
        CancellationToken cancellationToken)
    {
        var interviewOrder = new InterviewOrder(Guid.NewGuid(), request.UserId, request.InterviewDate,
            request.ProgrammingLanguage,
            request.Technologies);

        await _interviewOrderRepository.AddInterviewOrderAsync(interviewOrder);

        return _mapper.Map<InterviewOrderDto>(interviewOrder);
    }
}