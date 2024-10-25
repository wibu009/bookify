namespace Bookify.Domain.Users;

public sealed class Permission
{
    public Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public int Id { get; init; }
    public string Name { get; init; }
    public ICollection<Role> Roles { get; } = new List<Role>();
}