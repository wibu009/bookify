using Bookify.Application.Abstractions.Time;
using Bookify.Domain.Apartments.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Apartments.CreateApartment;

internal sealed class ApartmentCreatedDomainEventHandler(
    ILogger<ApartmentCreatedDomainEventHandler> logger,
    IDateTimeProvider dateTimeProvider)
    : INotificationHandler<ApartmentCreatedDomainEvent>
{
    public Task Handle(ApartmentCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        //Add some logic here
        logger.LogInformation("Apartment {Apartment} has been created at {Time}", notification.ApartmentId, dateTimeProvider.UtcNow);
        return Task.CompletedTask;
    }
}