using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MockInterview.Identity.Application.Abstractions.Interfaces;
using MockInterview.Identity.Application.Abstractions.Models;
using MockInterview.Identity.Application.Authentication.Models;
using MockInterview.Identity.Application.Common.Models.Users;
using MockInterview.Identity.Contracts.Events;
using MockInterview.Identity.DataAccess;
using MockInterview.Identity.Domain.Common.Enumerations;
using MockInterview.Identity.Domain.Entities.Users;
using Shared.Messaging;
using Shared.Security.Authentication.Interfaces;
using Shared.Security.Authentication.Models;

namespace MockInterview.Identity.Application.Authentication.GitHub.Commands;

public class AuthenticateGitHubUserCommandHandler
    : IRequestHandler<AuthenticateGitHubUserCommand, Result<AuthenticationResult>>
{
    private readonly IdentityDbContext _dbContext;
    private readonly IGitHubAuthClient _gitHubAuthClient;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public AuthenticateGitHubUserCommandHandler(IGitHubAuthClient gitHubAuthClient, IdentityDbContext dbContext,
        IJwtTokenGenerator jwtTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator, IMapper mapper,
        IEventBus eventBus)
    {
        _gitHubAuthClient = gitHubAuthClient;
        _dbContext = dbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    //Creates a new user if the user does not exist in the database otherwise updates the user's access token and refresh token
    public async Task<Result<AuthenticationResult>> Handle(AuthenticateGitHubUserCommand request,
        CancellationToken cancellationToken)
    {
        var gitHubTokenResult = await _gitHubAuthClient.ExchangeCodeForAccessToken(request.Code);

        var gitHubUser = await _gitHubAuthClient.GetUser(gitHubTokenResult.AccessToken);

        var existingUser =
            await _dbContext.Users
                .Include(x => x.Credentials)
                .FirstOrDefaultAsync(x =>
                        x.Credentials.Any(c =>
                            c.Provider == AuthProvider.GitHub
                            && c.ProviderUserId == gitHubUser.Id.ToString()),
                    cancellationToken);

        string userAccessToken;

        if (existingUser is not null)
        {
            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            existingUser.SetRefreshToken(refreshToken.Token, refreshToken.ExpiryTime);

            existingUser.Credentials
                .First(c => c.Provider == AuthProvider.GitHub)
                .UpdateAccessToken(gitHubTokenResult.AccessToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            userAccessToken =
                _jwtTokenGenerator.GenerateToken(new UserClaims(existingUser.Id, existingUser.Email));

            return new AuthenticationResult(_mapper.Map<UserDto>(existingUser), userAccessToken,
                existingUser.RefreshToken,
                existingUser.RefreshTokenExpiryTime, false);
        }

        var createdUser = CreateNewUser(gitHubUser, gitHubTokenResult.AccessToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _eventBus.PublishAsync(
            new UserCreatedEvent(createdUser.Id, createdUser.Email, createdUser.Username),
            cancellationToken);

        userAccessToken = _jwtTokenGenerator.GenerateToken(new UserClaims(createdUser.Id, createdUser.Email));

        return new AuthenticationResult(_mapper.Map<UserDto>(createdUser), userAccessToken, createdUser.RefreshToken,
            createdUser.RefreshTokenExpiryTime, true);
    }

    private User CreateNewUser(GitHubUser gitHubUser, string accessToken)
    {
        var user = User.CreateWithAuthProvider(gitHubUser.Name, gitHubUser.Login, gitHubUser.Email,
            gitHubUser.AvatarUrl, gitHubUser.Location, gitHubUser.Bio,
            AuthProvider.GitHub, accessToken, gitHubUser.Id.ToString());

        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken.Token, refreshToken.ExpiryTime);

        _dbContext.Users.Add(user);

        return user;
    }
}