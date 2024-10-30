namespace Bookify.Shared.Authorization;

public static class Roles
{
    public const string Basic = "basic";
    public const string Admin = "admin";
    
    public static string[] Default => [Basic, Admin];
    public static bool IsDefault(string role) => Default.Contains(role);
}