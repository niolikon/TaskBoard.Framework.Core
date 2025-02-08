using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using TaskBoard.Framework.Core.Exceptions.Rest;
using TaskBoard.Framework.Core.Security.Authentication;

namespace TaskBoard.Framework.Core.Security.Authentication;

public class AuthenticatedUserMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticatedUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        ClaimsPrincipal principal = context.User;
        if (principal.Identity?.IsAuthenticated == true)
        {
            AuthenticatedUser user = AuthenticatedUserMapper.MapToAuthenticatedUser(context.User);
            IAuthenticatedUserService userService = serviceProvider.GetRequiredService<IAuthenticatedUserService>();
            userService.User = user;
        }

        await _next(context);
    }
}
