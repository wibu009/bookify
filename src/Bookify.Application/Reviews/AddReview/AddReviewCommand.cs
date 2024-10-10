using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Reviews.AddReview;

public record AddReviewCommand(
    Guid BookingId,
    int Rating,
    string Comment) : ICommand<Guid>;