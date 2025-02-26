﻿using System.Text.Json.Serialization;

namespace TaskBoard.Framework.Core.Security.Keycloak;

public class KeycloakTokenResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int? ExpiresIn { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("refresh_expires_in")]
    public int? RefreshExpiresIn { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }
}
