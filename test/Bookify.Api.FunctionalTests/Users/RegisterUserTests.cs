using System.Net;
using System.Net.Http.Json;
using Bookify.Api.Controllers.Users;
using Bookify.Api.FunctionalTests.Infrastructure;

namespace Bookify.Api.FunctionalTests.Users;

public class RegisterUserTests : BaseFunctionalTest
{
    public RegisterUserTests(FunctionalTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Register_Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterUserRequest("create@test.com", "first", "last", "12345");
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/v1/users/register", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("", "first", "last", "12345")]
    [InlineData("test.com", "first", "last", "12345")]
    [InlineData("@test.com", "first", "last", "12345")]
    [InlineData("test@", "first", "last", "12345")]
    [InlineData("test@test.com", "", "last", "12345")]
    [InlineData("test@test.com", "first", "", "12345")]
    [InlineData("test@test.com", "first", "last", "")]
    [InlineData("test@test.com", "first", "last", "1")]
    [InlineData("test@test.com", "first", "last", "12")]
    [InlineData("test@test.com", "first", "last", "123")]
    [InlineData("test@test.com", "first", "last", "1234")]
    public async Task Register_Should_ReturnBadRequest_WhenRequestIsInvalid(string email, string firstName, string lastName, string password)
    {
        // Arrange
        var request = new RegisterUserRequest(email, firstName, lastName, password);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/v1/users/register", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }
}