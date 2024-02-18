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
                context.FireTimeUtc.UtcDateTime.AddHours(1));

        if (interviewOrders.Count < 2)
            return;

        var bestMatches = _matchingService.GetBestMatches(interviewOrders).ToList();

        foreach (var (first, second) in bestMatches)
        {
            await _eventBus
                .PublishAsync(new MatchFound(first.Id, second.Id,
                    first.CandidateId, second.CandidateId,
                    first.StartDateTime, first.ProgrammingLanguage, first.Technologies.Intersect(second.Technologies)));

            await _interviewOrderRepository.CloseMatchInterviewOrdersAsync(first.Id, second.Id);
        }
    }
}