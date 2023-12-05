using FluentResults;
using MediatR;
using MockInterview.Identity.Application.Authentication.Models;

namespace MockInterview.Identity.Application.Authentication.Account.Commands;

public record RegisterByEmailCommand
    (string Email, string Username, string Password) : IRequest<Result<AuthenticationResult>>;