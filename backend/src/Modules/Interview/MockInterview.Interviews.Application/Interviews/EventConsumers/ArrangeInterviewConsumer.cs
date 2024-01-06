using MassTransit;
using MockInterview.Interviews.Application.Abstractions.Repositories;
using MockInterview.Interviews.Contracts.Commands;
using MockInterview.Interviews.Contracts.Events;
using MockInterview.Interviews.Domain.Entities;

namespace MockInterview.Interviews.Application.Interviews.EventConsumers;

public class ArrangeInterviewConsumer : IConsumer<ArrangeInterview>
{
    private readonly IInterviewRepository _interviewRepository;

    public ArrangeInterviewConsumer(IInterviewRepository interviewRepository)
    {
        _interviewRepository = interviewRepository;
    }

    public async Task Consume(ConsumeContext<ArrangeInterview> context)
    {
        var command = context.Message;
        var interview = Interview.Create(command.FirstCandidateId, command.SecondCandidateId,
            command.StartDateTime, command.ProgrammingLanguage, command.Technologies);

        await _interviewRepository.AddAsync(interview);

        await context.Publish(new InterviewArranged(command.InterviewOrderId, interview.Id.ToString()));
    }
}