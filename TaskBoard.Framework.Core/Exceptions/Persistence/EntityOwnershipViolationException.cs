namespace TaskBoard.Framework.Core.Exceptions.Persistence;

public class EntityOwnershipViolationException(string message) : Exception(message)
{
}
