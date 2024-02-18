using FluentResults;
using MediatR;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Application.Skills.Models;
using MockInterview.Matchmaking.Domain.Models;

namespace MockInterview.Matchmaking.Application.Skills.Queries;

public class GetUserSkillsQueryHandler : IRequestHandler<GetUserSkillsQuery, Result<UserSkillsDto>>
{
    private readonly ISkillRepository _skillRepository;

    public GetUserSkillsQueryHandler(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }

    public async Task<Result<UserSkillsDto>> Handle(GetUserSkillsQuery request,
        CancellationToken cancellationToken)
    {
        var programmingLanguagesTask = _skillRepository.GetUsersProgrammingLanguagesAsync(request.UserId);
        
        var technologiesTask = _skillRepository.GetUserTechnologiesWithLanguagesAsync(request.UserId);
        
        await Task.WhenAll(programmingLanguagesTask, technologiesTask);

        return new UserSkillsDto(await programmingLanguagesTask, await technologiesTask);
    }
}