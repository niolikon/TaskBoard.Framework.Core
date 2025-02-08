namespace TaskBoard.Framework.Core.Exceptions.Persistence;

public class RepositorySaveChangeFailedException(string message) : Exception(message)
{
}
