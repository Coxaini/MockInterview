using FluentResults;
using MediatR;
using MockInterview.Identity.Application.Authentication.Models;

namespace MockInterview.Identity.Application.Authentication.GitHub.Commands;

public record AuthenticateGitHubUserCommand(string Code) : IRequest<Result<AuthenticationResult>>;