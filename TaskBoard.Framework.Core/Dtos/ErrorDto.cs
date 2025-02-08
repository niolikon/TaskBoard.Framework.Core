using TaskBoard.Framework.Core.Exceptions;

namespace TaskBoard.Framework.Core.Dtos;

public class ErrorDto
{
    public string Error { get; init; }
    public int Code { get; init; }

    public ErrorDto(BaseRestException restException)
    {
        Error = restException.ErrorMessage;
        Code = restException.ErrorCode;
    }

    public ErrorDto(string errorMessage, int errorCode)
    {
        Error = errorMessage;
        Code = errorCode;
    }
}
