using FluentResults;
using MediatR;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;

namespace MockInterview.Matchmaking.Application.Skills.Queries;

public class GetUserSkillsQueryHandler : IRequestHandler<GetUserSkillsQuery, Result<IEnumerable<string>>>
{
    private readonly ISkillRepository _skillRepository;

    public GetUserSkillsQueryHandler(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }

    public async Task<Result<IEnumerable<string>>> Handle(GetUserSkillsQuery request,
        CancellationToken cancellationToken)
    {
        var skills = await _skillRepository.GetUsersSkillsAsync(request.UserId);

        return Result.Ok(skills.Select(s => s.Name));
    }
}