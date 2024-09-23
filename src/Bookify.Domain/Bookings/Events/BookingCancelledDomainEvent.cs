using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public record BookingCancelledDomainEvent(Guid BookingId) : IDomainEvent;