using Bookify.Application.Abstractions.Time;
using Bookify.Domain.Reviews.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Reviews.AddReview;

internal sealed class ReviewCreatedDomainEventHandler(
    ILogger<ReviewCreatedDomainEventHandler> logger,
    IDateTimeProvider dateTimeProvider)
    : INotificationHandler<ReviewCreatedDomainEvent>
{

    public Task Handle(ReviewCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        //add some logic here
        logger.LogInformation("Review {Review} has been created at {Time}", notification.ReviewId, dateTimeProvider.UtcNow);
        return Task.CompletedTask;
    }
}