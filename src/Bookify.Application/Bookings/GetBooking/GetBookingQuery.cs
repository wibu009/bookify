using Bookify.Application.Abstractions.Caching;

namespace Bookify.Application.Bookings.GetBooking;

public sealed record GetBookingQuery(Guid BookingId) : ICacheQuery<BookingResponse>
{
    public string CacheKey => $"booking_{BookingId}";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}