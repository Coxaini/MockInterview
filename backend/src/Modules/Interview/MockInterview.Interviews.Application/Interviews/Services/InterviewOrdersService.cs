using MapsterMapper;
using MassTransit;
using MockInterview.Interviews.Application.Interviews.Models;
using MockInterview.Matchmaking.Contracts.Requests;

namespace MockInterview.Interviews.Application.Interviews.Services;

public class InterviewOrdersService : IInterviewOrdersService
{
    private readonly IMapper _mapper;
    private readonly IRequestClient<GetInterviewOrderRequest> _requestClient;

    public InterviewOrdersService(IRequestClient<GetInterviewOrderRequest> requestClient, IMapper mapper)
    {
        _requestClient = requestClient;
        _mapper = mapper;
    }

    public async Task<InterviewOrderDto?> GetInterviewOrderAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var response =
            await _requestClient.GetResponse<GetInterviewOrderResponse, InterviewOrderNotFound>(
                new GetInterviewOrderRequest(id), cancellationToken);

        if (response.Is(out Response<GetInterviewOrderResponse> getInterviewOrderResponse))
            return _mapper.Map<InterviewOrderDto>(getInterviewOrderResponse.Message);

        return null;
    }
}