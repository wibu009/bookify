using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Abstractions.Time;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;
using Bookify.Shared.Core;

namespace Bookify.Application.Reviews.AddReview;

internal sealed class AddReviewCommandHandler(
    IBookingRepository bookingRepository,
    IReviewRepository reviewRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<AddReviewCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
        if (booking is null)
        {
            return Result.Failure<Guid>(BookingErrors.NotFound);
        }
        
        var ratingResult = Rating.Create(request.Rating);
        if (ratingResult.IsFailure)
        {
            return Result.Failure<Guid>(ratingResult.Error);
        }
        
        var reviewResult = Review.Create(
            booking,
            ratingResult.Value,
            new Comment(request.Comment),
            dateTimeProvider.UtcNow);
        if (reviewResult.IsFailure)
        {
            return Result.Failure<Guid>(reviewResult.Error);
        }
        
        reviewRepository.Add(reviewResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return reviewResult.Value.Id;
    }
}