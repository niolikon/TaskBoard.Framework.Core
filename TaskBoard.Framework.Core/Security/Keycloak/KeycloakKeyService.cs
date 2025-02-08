using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace TaskBoard.Framework.Core.Security.Keycloak;

class KeycloakKeyService : IKeycloakKeyService
{
    private readonly ILogger<KeycloakKeyService> _logger;
    private readonly KeycloakOptions _options;
    private readonly JsonWebKeySet _jsonWebKeySet;

    public KeycloakKeyService(IOptions<KeycloakOptions> options, ILogger<KeycloakKeyService> logger)
    {
        _logger = logger;
        _options = options.Value.Verified();
        _jsonWebKeySet = InitKeySet(_options);
    }

    private JsonWebKeySet InitKeySet(KeycloakOptions keycloakOptions)
    {
        using var httpClient = new HttpClient();
        var jwksUri = $"{keycloakOptions.RealmUri}/protocol/openid-connect/certs";

        JsonWebKeySet result;
        try
        {
            _logger.LogInformation($"Fetching JWKS keys from {jwksUri}");

            var jwksResponse = httpClient.GetStringAsync(jwksUri).Result;
            result = new JsonWebKeySet(jwksResponse);

            _logger.LogInformation($"Fetching JWKS keys from {jwksUri} completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fetching JWKS keys from {jwksUri} could not be completed");
            result = new JsonWebKeySet();
        }

        return result;
    }


    public JsonWebKeySet GetKeySet()
    {
        return _jsonWebKeySet;
    }
}
