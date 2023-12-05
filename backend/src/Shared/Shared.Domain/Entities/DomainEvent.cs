using MediatR;

namespace Shared.Domain.Entities;

public record DomainEvent(Guid Id) : INotification;