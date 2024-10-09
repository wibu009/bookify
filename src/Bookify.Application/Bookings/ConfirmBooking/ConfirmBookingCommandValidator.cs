using FluentValidation;

namespace Bookify.Application.Bookings.ConfirmBooking;

public class ConfirmBookingCommandValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingCommandValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
    }
}