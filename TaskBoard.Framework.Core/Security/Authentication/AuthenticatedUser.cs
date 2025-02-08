namespace TaskBoard.Framework.Core.Security.Authentication;

public class AuthenticatedUser
{
    public required string Id { get; set; }

    public override string ToString()
    {
        return $"AuthenticatedUser() {{Id='{Id}'}}";
    }
}
