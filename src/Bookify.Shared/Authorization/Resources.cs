namespace Bookify.Shared.Authorization;

public static class Resources
{
    public const string Users = "users";
    public const string Roles = "roles";
    public const string Bookings = "bookings";
    public const string Apartments = "apartments";
    public const string Reviews = "reviews";
    
    public static string[] All => [Users, Roles, Bookings, Apartments, Reviews];
}