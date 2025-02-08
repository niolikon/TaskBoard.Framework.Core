using System.Net;

namespace TaskBoard.Framework.Core.Exceptions.Rest;

public class UnauthorizedRestException(string message) : BaseRestException(message, HttpStatusCode.Unauthorized)
{
}
