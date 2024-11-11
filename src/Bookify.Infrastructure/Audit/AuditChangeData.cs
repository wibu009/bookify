using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Bookify.Infrastructure.Audit;

public sealed class AuditChangeData
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        Formatting = Formatting.None,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    public Guid Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public Guid UserId { get; set; }
    public Dictionary<string, object?> KeyValues { get; set; } = new();
    public Dictionary<string, object?> OldValues { get; set; } = new();
    public Dictionary<string, object?> NewValues { get; set; } = new();
    public Collection<string> ModifiedProperties { get; set; } = new();
    public AuditActionType Type { get; set; }
    public string? TableName { get; set; }
    
    public AuditLogEntry ToAuditLogEntry() 
        => new()
        {
            Id = Guid.NewGuid(),
            UserId = UserId,
            Operation = Type.ToString(),
            Entity = TableName,
            DateTime = Timestamp,
            PrimaryKey = JsonConvert.SerializeObject(KeyValues, JsonSerializerSettings),
            PreviousValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues, JsonSerializerSettings),
            NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues, JsonSerializerSettings),
            ModifiedProperties = ModifiedProperties.Count == 0 ? null : JsonConvert.SerializeObject(ModifiedProperties, JsonSerializerSettings)
        };
}