namespace Bookify.Shared.Authorization;

public static class Roles
{
    public const string Basic = nameof(Basic.ToLower);
    public const string Admin = nameof(Admin.ToLower);
    
    public static string[] Default => [Basic, Admin];
    public static bool IsDefault(string role) => Default.Contains(role);
}