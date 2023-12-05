using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Identity.Application.Abstractions.Interfaces;
using MockInterview.Identity.Application.Authentication.Common.Errors;
using MockInterview.Identity.Application.Authentication.Models;
using MockInterview.Identity.Application.Common.Models.Users;
using MockInterview.Identity.Contracts.Events;
using MockInterview.Identity.DataAccess;
using MockInterview.Identity.Domain.Entities.Users;
using Shared.Messaging;
using Shared.Security.Authentication.Interfaces;
using Shared.Security.Authentication.Models;

namespace MockInterview.Identity.Application.Authentication.Account.Commands;

public class RegisterByEmailCommandHandler : IRequestHandler<RegisterByEmailCommand, Result<AuthenticationResult>>
{
    private readonly IdentityDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public RegisterByEmailCommandHandler(IdentityDbContext dbContext, IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator, IMapper mapper,
        IEventBus eventBus)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task<Result<AuthenticationResult>> Handle(RegisterByEmailCommand request,
        CancellationToken cancellationToken)
    {
        if (await _dbContext.Users.AnyAsync(x => x.Email == request.Email, cancellationToken))
            return Result.Fail(AuthenticationErrors.UserWithSuchEmailAlreadyExists);

        var user = User.CreateWithPassword(request.Email, request.Username,
            _passwordHasher.HashPassword(request.Password));
        _dbContext.Users.Add(user);

        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();
        user.SetRefreshToken(refreshToken.Token, refreshToken.ExpiryTime);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _eventBus.PublishAsync(new UserCreatedEvent(user.Id, user.Email, user.Username),
            CancellationToken.None);

        string accessToken = _jwtTokenGenerator.GenerateToken(new UserClaims(user.Id, user.Email));

        var result = new AuthenticationResult(_mapper.Map<UserDto>(user), accessToken, refreshToken.Token,
            refreshToken.ExpiryTime, true);

        return result;
    }
}