using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Abstractions.Time;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;

namespace Bookify.Application.Bookings.CompleteBooking;


internal sealed class CompleteBookingCommandHandler(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CompleteBookingCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CompleteBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
        if (booking is null)
        {
            return Result.Failure<Guid>(BookingErrors.NotFound);
        }
        
        var result = booking.Complete(dateTimeProvider.UtcNow);
        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return booking.Id;
    }
}