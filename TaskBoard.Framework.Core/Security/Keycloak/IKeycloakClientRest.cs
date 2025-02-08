namespace TaskBoard.Framework.Core.Security.Keycloak;

public interface IKeycloakClientRest
{
    public Task<TResponse> PostForObjectAsync<TResponse>(string requestUrl, FormUrlEncodedContent encodedContent);

    public Task<KeycloakResponseStatus> PostAsync(string requestUrl, FormUrlEncodedContent encodedContent);
}
