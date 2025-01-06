using System.Net.Http.Json;
using Bookify.Api.Endpoints.Users;
using Bookify.Api.FunctionalTests.Users;
using Bookify.Application.Users;

namespace Bookify.Api.FunctionalTests.Infrastructure;

public abstract class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
{
    protected readonly HttpClient HttpClient;

    protected BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
    }

    protected async Task<string> GetAccessTokenAsync()
    {
        var loginResponse = await HttpClient.PostAsJsonAsync("api/v1/users/login",
            new LogInUserRequest(
                UserData.RegisterTestUserRequest.Email,
                UserData.RegisterTestUserRequest.Password));

        var accessToken = await loginResponse.Content.ReadFromJsonAsync<TokenResponse>();

        return accessToken!.AccessToken;
    }
}