using Bookify.Application.Abstractions.Clock;
using Bookify.Domain.Bookings.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Bookings.CancelBooking;

internal sealed class BookingCancelledDomainEventHandler(
    ILogger<BookingCancelledDomainEventHandler> logger,
    IDateTimeProvider dateTimeProvider)
    : INotificationHandler<BookingCancelledDomainEvent>
{
    public Task Handle(BookingCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        //Add some logic here
        logger.LogInformation("Booking {Booking} has been canceled at {Time}", notification.BookingId, dateTimeProvider.UtcNow);
        return Task.CompletedTask;
    }
}