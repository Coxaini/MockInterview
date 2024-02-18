using FluentResults;
using MediatR;
using MockInterview.Matchmaking.Application.Interviews.Models;

namespace MockInterview.Matchmaking.Application.Interviews.Commands;

public record OrderInterviewSlotCommand(
    Guid UserId,
    string ProgrammingLanguage,
    IEnumerable<string> Technologies,
    DateTime InterviewDate) : IRequest<Result<InterviewOrderDto>>;