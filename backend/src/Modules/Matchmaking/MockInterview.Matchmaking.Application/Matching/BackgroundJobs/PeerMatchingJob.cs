using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Application.Matching.Services;
using MockInterview.Matchmaking.Contracts.Events;
using Quartz;
using Shared.Messaging;

namespace MockInterview.Matchmaking.Application.Matching.BackgroundJobs;

public class PeerMatchingJob : IJob
{
    private readonly IEventBus _eventBus;
    private readonly IInterviewOrderRepository _interviewOrderRepository;
    private readonly IMatchingService _matchingService;

    public PeerMatchingJob(IInterviewOrderRepository interviewOrderRepository, IMatchingService matchingService,
        IEventBus eventBus)
    {
        _interviewOrderRepository = interviewOrderRepository;
        _matchingService = matchingService;
        _eventBus = eventBus;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var interviewOrders =
            await _interviewOrderRepository.GetInterviewOrdersAtDateTimeAsync(
                context.ScheduledFireTimeUtc!.Value.UtcDateTime.AddHours(1));

        var interviewOrdersByProgrammingLanguage = interviewOrders
            .GroupBy(x => x.ProgrammingLanguage);

        foreach (var ordersGroup in interviewOrdersByProgrammingLanguage)
        {
            var interviewOrdersList = ordersGroup.ToList();

            if (interviewOrdersList.Count < 2)
                continue;

            var bestMatches = _matchingService.GetBestMatches(interviewOrdersList);

            foreach (var (first, second) in bestMatches)
                await _eventBus
                    .PublishAsync(new PeerMatchFound(first.Id, second.Id,
                        first.CandidateId, second.CandidateId,
                        first.StartDateTime, first.ProgrammingLanguage,
                        first.Technologies.Intersect(second.Technologies)));
        }
    }
}