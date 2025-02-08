using System.Text.Json;
using TaskBoard.Framework.Core.Exceptions.Rest;

namespace TaskBoard.Framework.Core.Security.Keycloak;

public class KeycloakApiClient : IKeycloakApiClient
{
    private readonly HttpClient _httpClient;

    public KeycloakApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TResponse> PostForObjectAsync<TResponse>(string requestUrl, FormUrlEncodedContent encodedContent)
    {
        var response = await _httpClient.PostAsync(requestUrl, encodedContent);

        if (!response.IsSuccessStatusCode)
            throw new UnauthorizedRestException("Could not perform request to keycloak");

        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TResponse>(content) ??
            throw new UnauthorizedRestException("Could not deserialize response from keycloak");
    }

    public async Task<KeycloakApiStatus> PostAsync(string requestUrl, FormUrlEncodedContent encodedContent)
    {
        var response = await _httpClient.PostAsync(requestUrl, encodedContent);

        if (!response.IsSuccessStatusCode)
            throw new UnauthorizedRestException("Could not perform request to keycloak");

        return new KeycloakApiStatus() { Success = response.IsSuccessStatusCode, StatusCode = (int) response.StatusCode };
    }
}
