using System.Net.Http.Headers;
using System.Net.Http.Json;
using Bookify.Infrastructure.Authentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authentication;

public sealed class AdminAuthorizationDelegatingHandler(IOptions<KeycloakOptions> keycloakOptions) : DelegatingHandler
{
    private readonly KeycloakOptions _keycloakOptions = keycloakOptions.Value;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authorizationToken = await GetAuthorizationToken(cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, authorizationToken.AccessToken);
        
        var httpResponseMessage = await base.SendAsync(request, cancellationToken);
        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed with status code {httpResponseMessage.StatusCode}. Response: {responseContent}");
        }

        return httpResponseMessage;
    }

    private async Task<AuthorizationTokenModel> GetAuthorizationToken(CancellationToken cancellationToken)
    {
        var authorizationRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _keycloakOptions.AdminClientId),
            new("client_secret", _keycloakOptions.AdminClientSecret),
            new("scope", "openid email"),
            new("grant_type", "client_credentials")
        };

        var authorizationRequestContent = new FormUrlEncodedContent(authorizationRequestParameters);

        var authorizationRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_keycloakOptions.TokenUrl))
        {
            Content = authorizationRequestContent
        };
        
        var authorizationResponse = await base.SendAsync(authorizationRequest, cancellationToken);

        if (!authorizationResponse.IsSuccessStatusCode)
        {
            var errorContent = await authorizationResponse.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Failed to retrieve authorization token. Status code: {authorizationResponse.StatusCode}. Error: {errorContent}");
        }
        
        var token = await authorizationResponse.Content.ReadFromJsonAsync<AuthorizationTokenModel>(cancellationToken: cancellationToken);
        
        return token ?? throw new ApplicationException("Failed to deserialize the authorization token response.");
    }
}
