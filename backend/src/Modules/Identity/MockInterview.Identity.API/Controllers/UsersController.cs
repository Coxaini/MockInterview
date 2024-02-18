using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockInterview.Identity.API.Requests.Users;
using MockInterview.Identity.Application.Common.Models.Users;
using MockInterview.Identity.Application.Users.Commands;
using MockInterview.Identity.Application.Users.Queries;
using Shared.Core.API.Controllers;

namespace MockInterview.Identity.API.Controllers;

[Route("users")]
public class UsersController : ApiController
{
    public UsersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        var result = await Mediator.Send(new GetUserQuery(UserId));
        return MatchResult(result, Ok);
    }

    [HttpPost("profile")]
    public async Task<ActionResult<UserDto>> FillUserProfile(FillProfileRequest request)
    {
        var result = await Mediator.Send(new FillProfileCommand(UserId, request.Name,
            request.Location, request.Bio, request.YearsOfExperience));
        return MatchResult(result, Ok);
    }
}