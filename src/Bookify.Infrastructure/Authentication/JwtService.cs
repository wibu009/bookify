using System.Net.Http.Json;
using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Authentication.Models;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authentication;

internal sealed class JwtService : IJwtService
{
    private static readonly Error AuthenticationError = new("Keycloak.AuthenticationFailed", "Failed to acquire access token to do authentication failure handling");
    
    private readonly HttpClient _httpClient;
    private readonly KeycloakOptions _keycloakOptions;
    
    public JwtService(HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions)
    {
        _httpClient = httpClient;
        _keycloakOptions = keycloakOptions.Value;
    }


    public async Task<Result<string>> GetAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var authRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id", _keycloakOptions.AuthClientId),
                new("client_secret", _keycloakOptions.AuthClientSecret),
                new("scope", "openid email"),
                new("grant_type", "password"),
                new("username", email),
                new("password", password)
            };
            
            var authorizationRequestContext = new FormUrlEncodedContent(authRequestParameters);
            
            var response = await _httpClient.PostAsync("", authorizationRequestContext, cancellationToken);
            response.EnsureSuccessStatusCode();

            var authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationTokenModel>(cancellationToken: cancellationToken);
            if (authorizationToken is null)
                return Result.Failure<string>(AuthenticationError);
            
            return Result.Success(authorizationToken.AccessToken);
        }
        catch (HttpRequestException)
        {
            return Result.Failure<string>(AuthenticationError);
        }
    }
}