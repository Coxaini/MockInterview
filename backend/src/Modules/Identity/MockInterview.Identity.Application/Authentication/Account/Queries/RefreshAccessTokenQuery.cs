using FluentResults;
using MediatR;
using MockInterview.Identity.Application.Authentication.Models;

namespace MockInterview.Identity.Application.Authentication.Account.Queries;

public record RefreshAccessTokenQuery
    (string ExpiredToken, string RefreshToken) : IRequest<Result<RefreshTokenResult>>;