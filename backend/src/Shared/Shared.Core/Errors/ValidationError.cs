using Shared.Domain.Errors;

namespace Shared.Core.Errors;

public class ValidationError : AppError
{
    public ValidationError(string message, string errorCode) : base(message,
        errorCode, ErrorType.Validation)
    {
    }
}