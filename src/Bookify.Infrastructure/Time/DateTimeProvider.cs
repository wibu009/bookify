using Bookify.Application.Abstractions.Clock;

namespace Bookify.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}