using TaskBoard.Framework.Core.Security.Authentication;

namespace TaskBoard.Framework.Core.Security.Authentication;

public interface IAuthenticatedUserService
{
    AuthenticatedUser User { get; set; }
}
