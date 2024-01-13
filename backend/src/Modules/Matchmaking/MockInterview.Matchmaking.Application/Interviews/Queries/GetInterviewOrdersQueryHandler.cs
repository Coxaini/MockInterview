using FluentResults;
using MapsterMapper;
using MediatR;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Application.Interviews.Models;

namespace MockInterview.Matchmaking.Application.Interviews.Queries;

public class
    GetInterviewOrdersQueryHandler : IRequestHandler<GetInterviewOrdersQuery, Result<IEnumerable<InterviewOrderDto>>>
{
    private readonly IInterviewOrderRepository _interviewOrderRepository;
    private readonly IMapper _mapper;

    public GetInterviewOrdersQueryHandler(IInterviewOrderRepository interviewOrderRepository, IMapper mapper)
    {
        _interviewOrderRepository = interviewOrderRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<InterviewOrderDto>>> Handle(GetInterviewOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var interviewOrders = await _interviewOrderRepository.GetInterviewOrdersByUserIdAsync(request.UserId);

        return Result.Ok(_mapper.Map<IEnumerable<InterviewOrderDto>>(interviewOrders));
    }
}