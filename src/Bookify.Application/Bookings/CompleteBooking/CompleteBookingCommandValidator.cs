using FluentValidation;

namespace Bookify.Application.Bookings.CompleteBooking;

public class CompleteBookingCommandValidator : AbstractValidator<CompleteBookingCommand>
{
    public CompleteBookingCommandValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
    }
}