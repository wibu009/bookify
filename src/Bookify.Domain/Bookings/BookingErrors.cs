using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings;

public static class BookingErrors
{
    public static Error NotFound => new("Booking.Found", "The booking with the specified identifier could not be found");
    public static Error Overlap => new("Booking.OverLap", "The current booking is overlapping with an existing one");
    public static Error NotReserved => new("Booking.NotReserved", "The booking is not pending");
    public static Error NotConfirmed => new("Booking.NotConfirmed", "The booking is not confirmed");
    public static Error AlreadyStarted => new("Booking.AlreadyStarted", "The booking has already started");
}