using MediatR;
using MockInterview.Identity.Application.Abstractions.Interfaces;

namespace MockInterview.Identity.Application.Authentication.GitHub.Queries;

public class GetGitHubAuthEndpointQueryHandler : IRequestHandler<GetGitHubAuthEndpointQuery, string>
{
    private readonly IGitHubAuthClient _gitHubAuthClient;

    public GetGitHubAuthEndpointQueryHandler(IGitHubAuthClient gitHubAuthClient)
    {
        _gitHubAuthClient = gitHubAuthClient;
    }

    public Task<string> Handle(GetGitHubAuthEndpointQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_gitHubAuthClient.GetAuthorizationEndpoint());
    }
}