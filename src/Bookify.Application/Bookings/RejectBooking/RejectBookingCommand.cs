using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Bookings.RejectBooking;

public record RejectBookingCommand(Guid BookingId) : ICommand;