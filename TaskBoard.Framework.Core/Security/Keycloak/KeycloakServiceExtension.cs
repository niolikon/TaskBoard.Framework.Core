using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace TaskBoard.Framework.Core.Security.Keycloak;

public static class KeycloakServiceExtension
{
    public static IServiceCollection AddAuthenticationWithKeycloakConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        KeycloakConfig keycloakConfig = new KeycloakConfig(configuration.GetSection("Keycloak")).Verified();
        services.Configure<KeycloakConfig>(configuration.GetSection("Keycloak"));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = $"{keycloakConfig.RealmUri}";
            options.Audience = keycloakConfig.ClientId;
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = $"{keycloakConfig.RealmUri}",
                ValidAudience = keycloakConfig.ClientId,
                RoleClaimType = "realm_access"
            };
        });

        services.AddAuthorization();

        return services;
    }

}
