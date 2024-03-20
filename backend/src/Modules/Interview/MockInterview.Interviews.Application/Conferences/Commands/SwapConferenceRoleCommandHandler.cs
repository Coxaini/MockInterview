using FluentResults;
using MapsterMapper;
using MediatR;
using MockInterview.Interviews.Application.Conferences.Errors;
using MockInterview.Interviews.Application.Conferences.Models;
using MockInterview.Interviews.Domain.Enumerations;
using MockInterview.Interviews.Domain.Models;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace MockInterview.Interviews.Application.Conferences.Commands;

public class SwapConferenceRoleCommandHandler : IRequestHandler<SwapConferenceRoleCommand, Result<ConferenceSessionDto>>
{
    private readonly IRedisCollection<ConferenceSession> _conferenceSessionCollection;
    private readonly IMapper _mapper;

    public SwapConferenceRoleCommandHandler(IRedisConnectionProvider connectionProvider, IMapper mapper)
    {
        _mapper = mapper;
        _conferenceSessionCollection = connectionProvider.RedisCollection<ConferenceSession>();
    }

    public async Task<Result<ConferenceSessionDto>> Handle(SwapConferenceRoleCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _conferenceSessionCollection.FirstOrDefaultAsync(s => s.Id == request.ConferenceId);

        if (session is null)
            return Result.Fail(ConferenceErrors.ConferenceNotFound);

        session.SwapRoles();

        await _conferenceSessionCollection.SaveAsync();

        return _mapper.Map<ConferenceSessionDto>(session);
    }
}