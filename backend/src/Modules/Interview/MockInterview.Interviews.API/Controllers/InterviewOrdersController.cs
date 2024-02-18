using MediatR;
using Microsoft.AspNetCore.Mvc;
using MockInterview.Interviews.Application.InterviewOrders.Queries;
using MockInterview.Interviews.Application.Interviews.Models;
using Shared.Core.API.Controllers;

namespace MockInterview.Interviews.API.Controllers;

[Route("interview-orders")]
public class InterviewOrdersController : ApiController
{
    public InterviewOrdersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("{interviewOrderId:guid}")]
    public async Task<ActionResult<UpcomingInterviewDetailsDto>> GetInterviewOrder(Guid interviewOrderId)
    {
        var result = await Mediator.Send(new GetInterviewOrderQuery(UserId, interviewOrderId));

        return MatchResult(result, Ok);
    }
}