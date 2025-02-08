namespace TaskBoard.Framework.Core.Security.Keycloak;

public class KeycloakRequestBuilder
{
    private string? clientId;
    private string? clientSecret;
    private string? username;
    private string? password;
    private string? refreshToken;
    private string? grantType;

    private KeycloakRequestBuilder() 
    {
        clientId = null;
        clientSecret = null;
        username = null;
        password = null;
        refreshToken = null;
        grantType = null;
    }

    public static KeycloakRequestBuilder Builder()
    {
        return new KeycloakRequestBuilder();
    }

    public KeycloakRequestBuilder WithClientId(string clientId)
    {
        this.clientId = clientId;
        return this;
    }

    public KeycloakRequestBuilder WithClientSecret(string clientSecret)
    {
        this.clientSecret = clientSecret;
        return this;
    }

    public KeycloakRequestBuilder WithUsername(string username)
    {
        this.username = username;
        return this;
    }

    public KeycloakRequestBuilder WithPassword(string password)
    {
        this.password = password;
        return this;
    }

    public KeycloakRequestBuilder WithRefreshToken(string refreshToken)
    {
        this.refreshToken = refreshToken;
        return this;
    }

    public KeycloakRequestBuilder WithPasswordGrantType()
    {
        this.grantType = "password";
        return this;
    }

    public KeycloakRequestBuilder WithRefreshTokenGrantType()
    {
        this.grantType = "refresh_token";
        return this;
    }

    public FormUrlEncodedContent Build()
    {
        Dictionary<string, string> requestParameters = new();

        if (!string.IsNullOrEmpty(clientId))
        {
            requestParameters["client_id"] = clientId;
        }
        if (!string.IsNullOrEmpty(clientSecret))
        {
            requestParameters["client_secret"] = clientSecret;
        }
        if (!string.IsNullOrEmpty(username))
        {
            requestParameters["username"] = username;
        }
        if (!string.IsNullOrEmpty(password))
        {
            requestParameters["password"] = password;
        }
        if (!string.IsNullOrEmpty(refreshToken))
        {
            requestParameters["refresh_token"] = refreshToken;
        }
        if (!string.IsNullOrEmpty(grantType))
        {
            requestParameters["grant_type"] = grantType;
        }

        return new FormUrlEncodedContent(requestParameters);
    }
}
