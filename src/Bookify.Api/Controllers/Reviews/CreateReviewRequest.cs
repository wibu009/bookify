namespace Bookify.Api.Controllers.Reviews;

public record CreateReviewRequest(
    Guid BookingId,
    int Rating,
    string Comment);