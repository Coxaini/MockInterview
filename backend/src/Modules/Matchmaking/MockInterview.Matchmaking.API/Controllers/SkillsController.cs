using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockInterview.Matchmaking.Application.Skills.Queries;
using Shared.Core.API.Controllers;

namespace MockInterview.Matchmaking.API.Controllers;

[Route("skills")]
[AllowAnonymous]
public class SkillsController : ApiController
{
    public SkillsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("technologies")]
    public async Task<IActionResult> GetTechnologies()
    {
        var result = await Mediator.Send(new GetTechnologiesQuery());

        return MatchResult(result, Ok);
    }
}