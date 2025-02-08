using System.Net;

namespace TaskBoard.Framework.Core.Exceptions.Rest;

public class BadRequestRestException(string message) : BaseRestException(message, HttpStatusCode.BadRequest)
{
}
