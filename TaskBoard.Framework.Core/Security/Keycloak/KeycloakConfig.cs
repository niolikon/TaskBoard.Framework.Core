using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace TaskBoard.Framework.Core.Security.Keycloak;

public class KeycloakConfig
{
    public KeycloakConfig(IConfigurationSection configurationSection)
    {
        RealmUri = configurationSection["RealmUri"] ??
            throw new InvalidConfigurationException("Missing configuration Keycloak:KeycloakServerUrl");
        ClientId = configurationSection["ClientId"] ??
            throw new InvalidConfigurationException("Missing configuration Keycloak:ClientId");
        ClientSecret = configurationSection["ClientSecret"];
    }

    public KeycloakConfig() { }

    public string RealmUri { get; set; }
    public string ClientId { get; set; }
    public string? ClientSecret { get; set; }

    public KeycloakConfig Verified()
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
