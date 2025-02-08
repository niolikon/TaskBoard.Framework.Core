using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace TaskBoard.Framework.Core.Security.Keycloak;

public class KeycloakOptions
{
    public KeycloakOptions() { }

    public KeycloakOptions(IConfigurationSection configurationSection)
    {
        RealmUri = configurationSection["RealmUri"] ??
            throw new InvalidConfigurationException("Missing configuration Keycloak:KeycloakServerUrl");
        ClientId = configurationSection["ClientId"] ??
            throw new InvalidConfigurationException("Missing configuration Keycloak:ClientId");
        ClientSecret = configurationSection["ClientSecret"];
    }

    public string RealmUri { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string? ClientSecret { get; set; }

    public KeycloakOptions Verified()
    {
        if (string.IsNullOrEmpty(RealmUri))
        {
            throw new InvalidConfigurationException("Missing configuration Keycloak:KeycloakServerUrl");
        }
        if (string.IsNullOrEmpty(ClientId))
        {
            throw new InvalidConfigurationException("Missing configuration Keycloak:ClientId");
        }

        return this;
    }
}
