using System.Security.Claims;
using TaskBoard.Framework.Core.Entities;
using TaskBoard.Framework.Core.Exceptions.Rest;

namespace TaskBoard.Framework.Core.Security.Authentication;

public static class AuthenticatedUserMapper
{
    static public AuthenticatedUser MapToAuthenticatedUser(ClaimsPrincipal user)
    {
        string id = user.Claims.FirstOrDefault(c => c.Type == "sid")?.Value ??
            throw new UnauthorizedRestException("User has no valid Id");

        return new() { Id = id };
    }

    static public AuthenticatedUser MapToAuthenticatedUser(IOwnerEntity owner)
    {
        return new() { Id = owner.Id };
    }

    static public TOwnerEntity MapToOwnerEntity<TOwnerEntity>(AuthenticatedUser user)
        where TOwnerEntity : IOwnerEntity, new()
    {
        return new TOwnerEntity() { Id = user.Id };
    }
}
