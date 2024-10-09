using FluentValidation;

namespace Bookify.Application.Bookings.RejectBooking;

public class RejectBookingCommandValidator : AbstractValidator<RejectBookingCommand>
{
    public RejectBookingCommandValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
    }
}