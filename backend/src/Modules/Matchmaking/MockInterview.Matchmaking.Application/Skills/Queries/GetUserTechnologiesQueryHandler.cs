using FluentResults;
using MediatR;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Domain.Models;
using MockInterview.Matchmaking.Domain.Models.Skills;

namespace MockInterview.Matchmaking.Application.Skills.Queries;

public class GetUserTechnologiesQueryHandler : IRequestHandler<GetUserTechnologiesQuery, Result<IEnumerable<Technology>>>
{
    private readonly ISkillRepository _skillRepository;

    public GetUserTechnologiesQueryHandler(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }

    public async Task<Result<IEnumerable<Technology>>> Handle(GetUserTechnologiesQuery request, CancellationToken cancellationToken)
    {
        var technologies = await _skillRepository.GetUserTechnologiesWithLanguagesAsync(request.UserId);
        
        return Result.Ok(technologies);
    }
}