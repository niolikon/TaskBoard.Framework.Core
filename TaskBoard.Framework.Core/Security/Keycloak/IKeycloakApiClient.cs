namespace TaskBoard.Framework.Core.Security.Keycloak;

public interface IKeycloakApiClient
{
    public Task<TResponse> PostForObjectAsync<TResponse>(string requestUrl, FormUrlEncodedContent encodedContent);

    public Task<KeycloakApiStatus> PostAsync(string requestUrl, FormUrlEncodedContent encodedContent);
}
