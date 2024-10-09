using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Abstractions.Time;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;

namespace Bookify.Application.Bookings.RejectBooking;

internal sealed class RejectBookingCommandHandler(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<RejectBookingCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RejectBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
        if (booking is null)
        {
            return Result.Failure<Guid>(BookingErrors.NotFound);
        }
        
        var result = booking.Reject(dateTimeProvider.UtcNow);
        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return booking.Id;
    }
}