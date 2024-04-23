namespace Shared.Domain.Errors;

public enum ErrorType
{
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    AccessDenied,
    DomainError,
    DatabaseError
}