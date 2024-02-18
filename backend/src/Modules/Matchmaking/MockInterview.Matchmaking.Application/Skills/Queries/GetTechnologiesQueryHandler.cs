using FluentResults;
using MediatR;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Domain.Models;
using MockInterview.Matchmaking.Domain.Models.Skills;

namespace MockInterview.Matchmaking.Application.Skills.Queries;

public class GetTechnologiesQueryHandler : IRequestHandler<GetTechnologiesQuery, Result<IEnumerable<Technology>>>
{
    private readonly ISkillRepository _skillRepository;

    public GetTechnologiesQueryHandler(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }

    public async Task<Result<IEnumerable<Technology>>> Handle(GetTechnologiesQuery request,
        CancellationToken cancellationToken)
    {
        var technologies = await _skillRepository.GetTechnologiesWithLanguagesAsync();

        return Result.Ok(technologies);
    }
}