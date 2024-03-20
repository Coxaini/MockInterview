using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Shared.Core.API.Hubs;

[Authorize]
public class BaseHub<T> : Hub<T> where T : class
{
    private Guid GetUserId()
    {
        if (Context.UserIdentifier is { } userIdentifier && Guid.TryParse(userIdentifier, out var userId))
            return userId;

        throw new HubException("Cannot find user id in claims");
    }

    protected Guid UserId => GetUserId();

    protected ValueTask<TR> MatchResultAsync<TR, TT>(Result<TT> result, Func<TT, Task<TR>> onSuccess)
    {
        return result.IsSuccess
            ? new ValueTask<TR>(onSuccess(result.Value))
            : throw new HubException(result.Errors[0].Message);
    }
}