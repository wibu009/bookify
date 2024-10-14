using Bookify.Domain.Users;

namespace Bookify.Domain.UnitTests.Users;

public static class UserData
{
    public static readonly FirstName FirstName = new("First");
    public static readonly LastName LastName = new("Last");
    public static readonly Email Email = new("test@test.com");
}