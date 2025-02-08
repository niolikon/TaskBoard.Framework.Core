using TaskBoard.Framework.Core.Entities;
using TaskBoard.Framework.Core.Security.Authentication;

namespace TaskBoard.Framework.Core.Security.Jwt;

public interface IJwtTokenFactory
{
    JwtTokenDto CreateJwtToken(AuthenticatedUser user, DateTime expiration);

    JwtTokenDto CreateJwtToken(IOwnerEntity owner, DateTime expiration);
}
