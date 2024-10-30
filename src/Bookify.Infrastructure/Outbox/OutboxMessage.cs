namespace Bookify.Infrastructure.Outbox;

public sealed class OutboxMessage(Guid id, DateTime occurredOnUtc, string type, string content)
{
    public Guid Id { get; init; } = id;
    public DateTime OccurredOnUtc { get; init; } = occurredOnUtc;
    public string Type { get; init; } = type;
    public string Content { get; init; } = content;
    public DateTime? ProcessedOnUtc { get; init; }
    public string? Error { get; init; }
}