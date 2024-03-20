using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Shared.Core.API.Hubs;

public static class HubExtensions
{
    /// <summary>
    /// Tries to get user id from claims
    /// </summary>
    /// <param name="hub"></param>
    /// <returns>user id</returns>
    /// <exception cref="HubException">if user id cannot be found in claims</exception>
    public static Guid TryGetUserId(this Hub hub)
    {
        if (hub.Context.UserIdentifier is { } userIdentifier && Guid.TryParse(userIdentifier, out var userId))
            return userId;

        throw new HubException("Cannot find user id in claims");
    }
}