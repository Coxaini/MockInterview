using System.Security.Claims;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MockInterview.Identity.Application.Authentication.Common.Errors;
using MockInterview.Identity.Application.Authentication.Models;
using MockInterview.Identity.Application.Common.Errors;
using MockInterview.Identity.DataAccess;
using Shared.Security.Authentication.Interfaces;
using Shared.Security.Authentication.Models;

namespace MockInterview.Identity.Application.Authentication.Account.Queries;

public class RefreshAccessTokenQueryHandler : IRequestHandler<RefreshAccessTokenQuery, Result<RefreshTokenResult>>
{
    private readonly IdentityDbContext _dbContext;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RefreshAccessTokenQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IdentityDbContext dbContext)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _dbContext = dbContext;
    }

    public async Task<Result<RefreshTokenResult>> Handle(RefreshAccessTokenQuery request,
        CancellationToken cancellationToken)
    {
        ClaimsPrincipal principal;

        try
        {
            principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.ExpiredToken);
        }
        catch (SecurityTokenException)
        {
            return Result.Fail(AuthenticationErrors.InvalidAccessToken);
        }

        var userId = Guid.Parse(principal.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);

        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
        {
            return Result.Fail(UserErrors.UserNotFound);
        }

        if (user.RefreshToken != request.RefreshToken)
        {
            return Result.Fail(AuthenticationErrors.RefreshTokenExpired);
        }

        string newAccessToken = _jwtTokenGenerator.GenerateToken(new UserClaims(user.Id, user.Email));

        return new RefreshTokenResult(newAccessToken, user.RefreshToken, user.RefreshTokenExpiryTime);
    }
}