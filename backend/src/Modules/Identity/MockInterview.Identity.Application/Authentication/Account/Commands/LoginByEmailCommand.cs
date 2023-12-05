using FluentResults;
using MediatR;
using MockInterview.Identity.Application.Authentication.Models;

namespace MockInterview.Identity.Application.Authentication.Account.Commands;

public record LoginByEmailCommand(string Email, string Password) : IRequest<Result<AuthenticationResult>>;