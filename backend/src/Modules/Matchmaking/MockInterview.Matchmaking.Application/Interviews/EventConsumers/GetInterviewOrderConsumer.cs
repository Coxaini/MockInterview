using MassTransit;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Contracts.Requests;

namespace MockInterview.Matchmaking.Application.Interviews.EventConsumers;

public class GetInterviewOrderConsumer : IConsumer<GetInterviewOrderRequest>
{
    private readonly IInterviewOrderRepository _interviewOrderRepository;

    public GetInterviewOrderConsumer(IInterviewOrderRepository interviewOrderRepository)
    {
        _interviewOrderRepository = interviewOrderRepository;
    }

    public async Task Consume(ConsumeContext<GetInterviewOrderRequest> context)
    {
        var interviewOrder =
            await _interviewOrderRepository.GetInterviewOrderByIdAsync(context.Message.InterviewOrderId);

        if (interviewOrder is null)
            await context.RespondAsync(new InterviewOrderNotFound(context.Message.InterviewOrderId));
        else
            await context.RespondAsync(new GetInterviewOrderResponse(
                interviewOrder.Id,
                interviewOrder.CandidateId,
                interviewOrder.StartDateTime,
                interviewOrder.ProgrammingLanguage,
                interviewOrder.Technologies));
    }
}