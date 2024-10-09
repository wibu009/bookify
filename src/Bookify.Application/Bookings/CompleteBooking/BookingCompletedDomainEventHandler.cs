using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Abstractions.Time;
using Bookify.Domain.Bookings.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Bookings.CompleteBooking;

internal sealed class BookingCompletedDomainEventHandler(
    ILogger<BookingCompletedDomainEventHandler> logger,
    IDateTimeProvider dateTimeProvider)
    : INotificationHandler<BookingCompletedDomainEvent>
{
    public Task Handle(BookingCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        //Add some logic here
        logger.LogInformation("Booking {Booking} has been completed at {Time}", notification.BookingId, dateTimeProvider.UtcNow);
        return Task.CompletedTask;
    }
}