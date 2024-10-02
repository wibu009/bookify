using System.Net.Http.Json;
using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Authentication.Models;

namespace Bookify.Infrastructure.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
    private const string PasswordCredentialType = "password";
    private readonly HttpClient _httpClient;
    
    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        var userRepresentationModel = UserRepresentationModel.FromUser(user);

        userRepresentationModel.Credentials =
        [
            new CredentialRepresentationModel
            {
                Value = password,
                Temporary = false,
                Type = PasswordCredentialType
            }
        ];
        
        var response = await _httpClient.PostAsJsonAsync("users", userRepresentationModel, cancellationToken);
        
        response.EnsureSuccessStatusCode();

        return ExtractIdentityIdFromLocationHeader(response);
    }

    private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        var locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;
        if (locationHeader is null) throw new InvalidOperationException("Location header is missing");
        
        var usersSegmentValueIndex = locationHeader.IndexOf(usersSegmentName, StringComparison.InvariantCultureIgnoreCase);
        
        var userIdentityId = locationHeader[(usersSegmentValueIndex + usersSegmentName.Length)..];

        return userIdentityId;
    }
}