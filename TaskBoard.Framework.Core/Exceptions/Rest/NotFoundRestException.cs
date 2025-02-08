using System.Net;

namespace TaskBoard.Framework.Core.Exceptions.Rest;

public class NotFoundRestException(string message) : BaseRestException(message, HttpStatusCode.NotFound)
{
}
