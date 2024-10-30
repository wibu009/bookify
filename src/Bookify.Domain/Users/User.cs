using Bookify.Domain.Abstractions;
using Bookify.Domain.Users.Events;

namespace Bookify.Domain.Users;

public sealed class User : Entity
{
    private readonly IList<Role> _roles = new List<Role>();
    private User(Guid id, FirstName firstName, LastName lastName, Email email) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
    
    private User() { }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public string IdentityId { get; private set; } = string.Empty;
    public IReadOnlyList<Role> Roles => _roles.ToList();

    public static User Create(FirstName firstName, LastName lastName, Email email, Role? role = null)
    {
        var user = new User(Guid.NewGuid(), firstName, lastName, email);
        
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        
        user._roles.Add(new Role(1, Bookify.Shared.Authorization.Roles.Basic));
        
        if (role is not null)
        {
            user._roles.Add(role);
        }
        
        return user;
    }

    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }
}