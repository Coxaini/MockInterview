using MediatR;
using Microsoft.AspNetCore.Mvc;
using MockInterview.Matchmaking.API.Requests;
using MockInterview.Matchmaking.Application.Skills.Commands;
using MockInterview.Matchmaking.Application.Skills.Models;
using MockInterview.Matchmaking.Application.Skills.Queries;
using MockInterview.Matchmaking.Domain.Models;
using MockInterview.Matchmaking.Domain.Models.Skills;
using Shared.Core.API.Controllers;

namespace MockInterview.Matchmaking.API.Controllers;

[Route("user/skills")]
public class UserSkillsController : ApiController
{
    public UserSkillsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserSkills(UpdateSkillsRequest request)
    {
        var result =
            await Mediator.Send(new UpdateSkillsCommand(UserId,
                request.ProgrammingLanguages.Concat(request.Technologies)));

        return MatchResult(result, Ok);
    }

    [HttpGet]
    public async Task<ActionResult<UserSkillsDto>> GetUserSkills()
    {
        var result = await Mediator.Send(new GetUserSkillsQuery(UserId));

        return MatchResult(result, Ok);
    }
    
    [HttpGet("technologies")]
    public async Task<ActionResult<IEnumerable<Technology>>> GetUserTechnologies()
    {
        var result = await Mediator.Send(new GetUserTechnologiesQuery(UserId));

        return MatchResult(result, Ok);
    }
}