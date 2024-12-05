using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Time;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Bookify.Infrastructure.Persistence.Interceptors;

public sealed class AuditSaveChangesInterceptor(IUserContext userContext, IDateTimeProvider dateTimeProvider)
    : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        await SaveAuditLogsAsync(eventData);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task SaveAuditLogsAsync(DbContextEventData eventData)
    {
        if (eventData.Context == null) return;
        
        if (eventData.Context.ChangeTracker.Entries<AuditLogEntry>().Any()) return;

        eventData.Context.ChangeTracker.DetectChanges();
        var changes = new List<AuditChangeData>();
        var utcNow = dateTimeProvider.UtcNow;

        foreach (var entry in eventData.Context.ChangeTracker.Entries<Entity>()
                 .Where(x => x.State is EntityState.Added or EntityState.Deleted or EntityState.Modified))
        {
            var changeData = new AuditChangeData
            {
                Id = Guid.NewGuid(),
                TableName = entry.Entity.GetType().Name,
                UserId = userContext.UserId,
                Timestamp = utcNow
            };

            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary) continue;
                var propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    changeData.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }
                switch (entry.State)
                {
                    case EntityState.Added:
                        changeData.Type = AuditActionType.Create;
                        changeData.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        changeData.Type = AuditActionType.Delete;
                        changeData.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property is { IsModified: true, OriginalValue: null, CurrentValue: not null })
                        {
                            changeData.ModifiedProperties.Add(propertyName);
                            changeData.Type = AuditActionType.Delete;
                            changeData.OldValues[propertyName] = property.OriginalValue;
                            changeData.NewValues[propertyName] = property.CurrentValue;
                        }
                        else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                        {
                            changeData.ModifiedProperties.Add(propertyName);
                            changeData.Type = AuditActionType.Update;
                            changeData.OldValues[propertyName] = property.OriginalValue;
                            changeData.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }

            changes.Add(changeData);
        }

        if (changes.Count == 0) return;
        
        var auditLogs = changes.Select(change => change.ToAuditLogEntry()).ToList();
        await eventData.Context.Set<AuditLogEntry>().AddRangeAsync(auditLogs);
        await eventData.Context.SaveChangesAsync();
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;
        foreach (var entry in context.ChangeTracker.Entries<Entity>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified || HasChangedOwnedEntities(entry))
            {
                var utcNow =dateTimeProvider.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = userContext.UserId;
                    entry.Entity.Created = utcNow;
                }
                entry.Entity.LastModifiedBy = userContext.UserId;
                entry.Entity.LastModified = utcNow;
            }
        }
    }

    private static bool HasChangedOwnedEntities(EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}