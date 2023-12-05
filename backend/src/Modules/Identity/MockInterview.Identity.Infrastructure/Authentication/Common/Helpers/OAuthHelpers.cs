using Microsoft.AspNetCore.WebUtilities;
using MockInterview.Identity.Application.Abstractions.Models;

namespace MockInterview.Identity.Infrastructure.Authentication.Common.Helpers;

public static class OAuthHelpers
{
    public static OAuthTokenResult ParseOAuthQuery(string query)
    {
        var queryValues = QueryHelpers.ParseQuery(query);

        return new OAuthTokenResult(queryValues["access_token"]!, queryValues["token_type"]!, queryValues["scope"]!);
    }
}