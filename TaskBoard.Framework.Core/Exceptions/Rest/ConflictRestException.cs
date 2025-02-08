using System.Net;

namespace TaskBoard.Framework.Core.Exceptions.Rest;

public class ConflictRestException(string message) : BaseRestException(message, HttpStatusCode.Conflict)
{
}
