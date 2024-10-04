using System.Text.Json.Serialization;

namespace Bookify.Infrastructure.Authentication.Models;

public sealed class AuthorizationTokenModel
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;
}