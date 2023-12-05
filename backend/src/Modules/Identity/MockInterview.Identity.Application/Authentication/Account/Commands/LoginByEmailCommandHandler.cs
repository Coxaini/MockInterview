using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Identity.Application.Abstractions.Interfaces;
using MockInterview.Identity.Application.Authentication.Common.Errors;
using MockInterview.Identity.Application.Authentication.Models;
using MockInterview.Identity.Application.Common.Models.Users;
using MockInterview.Identity.DataAccess;
using Shared.Security.Authentication.Interfaces;
using Shared.Security.Authentication.Models;

namespace MockInterview.Identity.Application.Authentication.Account.Commands;

public class LoginByEmailCommandHandler : IRequestHandler<LoginByEmailCommand, Result<AuthenticationResult>>
{
    private readonly IdentityDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;


    public LoginByEmailCommandHandler(IdentityDbContext dbContext, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator, IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<AuthenticationResult>> Handle(LoginByEmailCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user is null)
            return Result.Fail(AuthenticationErrors.IncorrectEmailOrPassword);

        if (user.PasswordHash is null)
            return Result.Fail(AuthenticationErrors.IncorrectEmailOrPassword);

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return Result.Fail(AuthenticationErrors.IncorrectEmailOrPassword);

        string token = _jwtTokenGenerator.GenerateToken(new UserClaims(user.Id, user.Email));
        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken.Token, refreshToken.ExpiryTime);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AuthenticationResult(_mapper.Map<UserDto>(user), token, refreshToken.Token, refreshToken.ExpiryTime,
            false);
    }
}