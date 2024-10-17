using Bookify.Api.Controllers.Users;

namespace Bookify.Api.FunctionalTests.Users;

internal static class UserData
{
    public static string FirstName => "test";
    public static string LastName => "test";
    public static string Email => "test@test.com";
    public static string Password => "12345";
    public static RegisterUserRequest RegisterTestUserRequest => new(Email, FirstName, LastName, Password);
}