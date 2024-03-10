using System.Security.Claims;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Core.Errors;
using Shared.Domain.Errors;

namespace Shared.Core.API.Controllers;

[ApiController]
[Authorize]
public abstract class ApiController : ControllerBase
{
    protected IMediator Mediator { get; }

    protected ApiController(IMediator mediator)
    {
        Mediator = mediator;
    }

    protected Guid UserId
    {
        get
        {
            string? userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool isParsed = Guid.TryParse(userId, out var result);

            return !isParsed ? throw new InvalidOperationException("Cannot find user id in claims") : result;
        }
    }

    protected ActionResult MatchResult<T>(Result<T> result, Func<T, ActionResult> onSuccess)
    {
        return result.IsSuccess ? onSuccess(result.Value) : Problem(result.Errors);
    }

    protected ValueTask<ActionResult> MatchResultAsync<T>(Result<T> result, Func<T, Task<ActionResult>> onSuccess)
    {
        return result.IsSuccess
            ? new ValueTask<ActionResult>(onSuccess(result.Value))
            : new ValueTask<ActionResult>(Problem(result.Errors));
    }

    protected ValueTask<ActionResult> MatchResultAsync(Result result, Func<Task<ActionResult>> onSuccess)
    {
        return result.IsSuccess
            ? new ValueTask<ActionResult>(onSuccess())
            : new ValueTask<ActionResult>(Problem(result.Errors));
    }

    protected ActionResult MatchResult(Result result, Func<ActionResult> onSuccess)
    {
        return result.IsSuccess ? onSuccess() : Problem(result.Errors);
    }

    private ActionResult Problem(List<IError> errors)
    {
        if (errors.Count is 0) return Problem();

        if (errors.All(error => error is ValidationError)) return ValidationProblem(errors);

        return Problem(errors[0]);
    }

    private ActionResult Problem(IError error)
    {
        int statusCode = error.Metadata["Type"] switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.AccessDenied => StatusCodes.Status403Forbidden,
            ErrorType.DomainError => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: error.Message);
    }

    private ActionResult ValidationProblem(List<IError> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in errors)
            modelStateDictionary.AddModelError(
                error.Metadata["Code"].ToString() ?? "Validation",
                error.Message);

        return ValidationProblem(modelStateDictionary);
    }
}