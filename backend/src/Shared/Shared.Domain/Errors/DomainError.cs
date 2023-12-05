using FluentResults;

namespace Shared.Domain.Errors;

public class DomainError : Error
{
    public DomainError(string message, string errorCode) : base(message)
    {
        Metadata.Add("Type", ErrorType.DomainError);
        Metadata.Add("Code", errorCode);
    }
}