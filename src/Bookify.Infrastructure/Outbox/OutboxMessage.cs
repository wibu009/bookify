namespace Bookify.Infrastructure.Outbox;

public sealed class OutboxMessage(Guid id, DateTime occurredOnUtc, string type, string content)
{
    public Guid Id { get; init; } = id;
    public DateTime OccurredOnUtc { get; set; } = occurredOnUtc;
    public string Type { get; set; } = type;
    public string Content { get; set; } = content;
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }
}