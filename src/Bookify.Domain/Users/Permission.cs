namespace Bookify.Domain.Users;

public sealed class Permission
{
    public static readonly Permission UsersRead = new(1, "users:read");
    public static readonly Permission UsersWrite = new(2, "users:write");
    
    public Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public int Id { get; init; }
    public string Name { get; init; }
    public ICollection<Role> Roles { get; } = new List<Role>();
}