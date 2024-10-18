using FluentValidation;

namespace Bookify.Application.Bookings.RejectBooking;

public sealed class RejectBookingCommandValidator : AbstractValidator<RejectBookingCommand>
{
    public RejectBookingCommandValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
    }
}