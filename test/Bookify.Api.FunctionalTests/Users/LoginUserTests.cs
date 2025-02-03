using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Bookify.Api.Endpoints.Users;
using Bookify.Api.FunctionalTests.Infrastructure;
using Bookify.Application.Users.GetLoggedInUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Bookify.Api.FunctionalTests.Users;

public class LoginUserTests : BaseFunctionalTest
{
    private const string InvalidEmail = "invalid@test.com";
    private const string InvalidPassword = "123456";
    
    public LoginUserTests(FunctionalTestWebAppFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Login_Should_ReturnUnauthorized_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new LogInUserRequest(InvalidEmail, UserData.Password);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/v1/users/login", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Login_Should_ReturnUnauthorized_WhenPasswordIsInvalid()
    {
        // Arrange
        var request = new LogInUserRequest(UserData.Email, InvalidPassword);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/v1/users/login", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Login_Should_ReturnOk_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LogInUserRequest(UserData.Email, UserData.Password);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/v1/users/login", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_Should_ReturnUser_WhenAccessTokenIsValid()
    {
        // Arrange
        var accessToken = await GetAccessTokenAsync();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);
        
        // Act
        var user = await HttpClient.GetFromJsonAsync<UserResponse>("api/v1/users/me");
        
        // Assert
        user.Should().NotBeNull();
    }
}