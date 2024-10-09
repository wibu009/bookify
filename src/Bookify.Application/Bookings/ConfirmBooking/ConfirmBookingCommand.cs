using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Bookings.ConfirmBooking;

public record ConfirmBookingCommand(Guid BookingId) : ICommand<Guid>;