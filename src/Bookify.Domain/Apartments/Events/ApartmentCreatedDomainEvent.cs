using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartments.Events;

public record ApartmentCreatedDomainEvent(Guid ApartmentId) : IDomainEvent;