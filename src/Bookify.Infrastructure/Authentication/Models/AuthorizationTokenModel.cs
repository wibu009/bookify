using System.Text.Json.Serialization;

namespace Bookify.Infrastructure.Authentication.Models;

public sealed class AuthorizationTokenModel
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; init; } = string.Empty;
    [JsonPropertyName("refresh_expires_in")]
    public int RefreshExpiresIn { get; init; }
    [JsonPropertyName("token_type")]
    public string TokenType { get; init; } = string.Empty;
}