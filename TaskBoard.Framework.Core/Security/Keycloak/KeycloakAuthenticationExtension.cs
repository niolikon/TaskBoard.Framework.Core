using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace TaskBoard.Framework.Core.Security.Keycloak;

public static class KeycloakAuthenticationExtension
{
    public static IServiceCollection AddAuthenticationWithKeycloakConfiguration(this IServiceCollection services, IConfigurationSection keycloakConfigurationSection)
    {
        services.Configure<KeycloakOptions>(keycloakConfigurationSection);

        services.AddSingleton<IKeycloakKeyService, KeycloakKeyService>();

        services.AddHttpClient();
        services.AddSingleton<IKeycloakApiClient, KeycloakApiClient>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var keycloakOptions = keycloakConfigurationSection.Get<KeycloakOptions>();

            options.Authority = $"{keycloakOptions.RealmUri}";
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidIssuer = $"{keycloakOptions.RealmUri}",
                ValidAudience = keycloakOptions.ClientId,
                RoleClaimType = "realm_access.roles"
            };

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var keycloakJwksService = context.HttpContext.RequestServices.GetRequiredService<IKeycloakKeyService>();
                    context.Options.TokenValidationParameters.IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                    {
                        return keycloakJwksService.GetKeySet().Keys;
                    };
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();

        return services;
    }

}
