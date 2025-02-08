using TaskBoard.Framework.Core.Exceptions.Rest;
using TaskBoard.Framework.Core.Security.Authentication;

namespace TaskBoard.Framework.Core.Security.Authentication;

public class AuthenticatedUserService : IAuthenticatedUserService
{
    private AuthenticatedUser? _user;
    public AuthenticatedUser User 
    {
        get { return _user ?? throw new UnauthorizedRestException("Error in user authentication"); }
        set { _user = value; }
    }
}
