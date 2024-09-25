namespace Bookify.Domain.Abstractions;

public abstract class Entity
{
    protected Entity(Guid id) => Id = id;
    
    public Guid Id { get; init; }
    
    private readonly List<IDomainEvent> _domainEvents = [];
    
    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}