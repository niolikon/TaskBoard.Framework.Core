using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace TaskBoard.Framework.Core.Security.Authentication;

public class AuthenticatedUserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticatedUserMiddleware> _logger;

    public AuthenticatedUserMiddleware(RequestDelegate next, ILogger<AuthenticatedUserMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        ClaimsPrincipal principal = context.User;

        _logger.LogDebug("Middleware Invoked. IsAuthenticated: {IsAuthenticated}", principal.Identity?.IsAuthenticated);

        if (principal.Identity?.IsAuthenticated == true)
        {
            _logger.LogDebug("User is authenticated. Extracting claims...");

            AuthenticatedUser user = AuthenticatedUserMapper.MapToAuthenticatedUser(context.User);
            IAuthenticatedUserService userService = serviceProvider.GetRequiredService<IAuthenticatedUserService>();
            userService.User = user;

            _logger.LogTrace("Authenticated user set: {User}", user);
        }
        else
        {
            _logger.LogDebug("User is NOT authenticated.");
        }

        await _next(context);
    }
}
