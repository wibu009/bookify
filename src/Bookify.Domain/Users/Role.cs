namespace Bookify.Domain.Users;

public sealed class Role
{
    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public int Id { get; init; }
    public string Name { get; init; }
    public ICollection<User> Users { get; } = new List<User>();
    public ICollection<Permission> Permissions { get; } = new List<Permission>();
}