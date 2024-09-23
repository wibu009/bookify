using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Reviews.Events;

public record ReviewCreatedDomainEvent(Guid ReviewId) : IDomainEvent;