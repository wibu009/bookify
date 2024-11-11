using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Bookify.Infrastructure.Audit;

public sealed class AuditLogEntry
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string? Operation { get; init; }
    public string? Entity { get; init; }
    public DateTimeOffset DateTime { get; init; }
    public string? PreviousValues { get; init; }
    public string? NewValues { get; init; }
    public string? ModifiedProperties { get; init; }
    public string? PrimaryKey { get; init; }

    public AuditChangeData ToAuditChangeData()
    {
        var keyValues = string.IsNullOrEmpty(PrimaryKey) ? new Dictionary<string, object?>() : JsonConvert.DeserializeObject<Dictionary<string, object?>>(PrimaryKey);
        var oldValues = string.IsNullOrEmpty(PreviousValues) ? new Dictionary<string, object?>() : JsonConvert.DeserializeObject<Dictionary<string, object?>>(PreviousValues);
        var newValues = string.IsNullOrEmpty(NewValues) ? new Dictionary<string, object?>() : JsonConvert.DeserializeObject<Dictionary<string, object?>>(NewValues);
        var modifiedProperties = string.IsNullOrEmpty(ModifiedProperties) ? [] : JsonConvert.DeserializeObject<Collection<string>>(ModifiedProperties);
        
        return new AuditChangeData
        {
            Id = Id,
            Timestamp = DateTime,
            UserId = UserId,
            KeyValues = keyValues!,
            OldValues = oldValues!,
            NewValues = newValues!,
            ModifiedProperties = modifiedProperties!,
            Type = Enum.TryParse<AuditActionType>(Operation, out var type) ? type : AuditActionType.None,
            TableName = Entity
        };
    }
}