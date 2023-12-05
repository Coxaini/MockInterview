using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Core.API.Controllers;

[AllowAnonymous]
[Route("auth")]
public class AuthorizationController : ApiController
{
    public AuthorizationController(IMediator mediator) : base(mediator)
    {
    }
}