using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.API.Controllers;

namespace MockInterview.Interviews.API.Controllers;

[ApiController]
[Route("interviews/{interviewId}/questions")]
public class InterviewQuestionsController : ApiController
{
    public InterviewQuestionsController(IMediator mediator) : base(mediator)
    {
    }
}