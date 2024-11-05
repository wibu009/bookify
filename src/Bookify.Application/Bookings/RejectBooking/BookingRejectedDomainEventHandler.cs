using Bookify.Application.Abstractions.Time;
using Bookify.Domain.Bookings.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Bookings.RejectBooking;

internal sealed class BookingRejectedDomainEventHandler(
    ILogger<BookingRejectedDomainEventHandler> logger,
    IDateTimeProvider dateTimeProvider)
    : INotificationHandler<BookingRejectedDomainEvent>
{
    public Task Handle(BookingRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        //Add some logic here
        logger.LogInformation("Booking {Booking} has been rejected at {Time}", notification.BookingId, dateTimeProvider.UtcNow);
        return Task.CompletedTask;
    }
}