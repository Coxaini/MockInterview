using MediatR;

namespace MockInterview.Identity.Application.Authentication.GitHub.Queries;

public record GetGitHubAuthEndpointQuery : IRequest<string>;