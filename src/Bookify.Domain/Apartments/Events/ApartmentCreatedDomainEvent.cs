using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartments.Events;

public sealed record ApartmentCreatedDomainEvent(Guid ApartmentId) : IDomainEvent;