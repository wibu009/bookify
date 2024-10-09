using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Email;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Bookings.ConfirmBooking;

internal sealed class BookingConfirmedDomainEventHandler(ILogger<BookingConfirmedDomainEventHandler> logger, IDateTimeProvider dateTimeProvider)
    : INotificationHandler<BookingConfirmedDomainEvent>
{
    public Task Handle(BookingConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        //Add some logic here
        logger.LogInformation("Booking {Booking} has been confirmed at {Time}", notification.BookingId, dateTimeProvider.UtcNow);
        return Task.CompletedTask;
    }
}
