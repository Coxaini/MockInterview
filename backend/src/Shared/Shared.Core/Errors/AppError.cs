using FluentResults;
using Shared.Domain.Errors;

namespace Shared.Core.Errors;

public class AppError : Error
{
    public AppError(string message, string errorCode, ErrorType errorType = ErrorType.Unexpected) : base(message)
    {
        Metadata.Add("Type", errorType);
        Metadata.Add("Code", errorCode);
    }
}