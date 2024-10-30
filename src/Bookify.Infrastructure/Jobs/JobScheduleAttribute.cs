using Bookify.Infrastructure.Caching;

namespace Bookify.Infrastructure.Jobs;

[AttributeUsage(AttributeTargets.Class)]
public class JobScheduleAttribute : Attribute
{
    public string? CronExpression { get; }
    public TimeSpan? Interval { get; }
    public int? RepeatCount { get; }
    
    public JobScheduleAttribute(string cronExpression)
    {
        CronExpression = cronExpression;
    }
    
    public JobScheduleAttribute(int hours, int minutes, int seconds, int repeatCount = -1)
    {
        Interval = new TimeSpan(hours, minutes, seconds);
        RepeatCount = repeatCount < 0 ? null : repeatCount;
    }
}