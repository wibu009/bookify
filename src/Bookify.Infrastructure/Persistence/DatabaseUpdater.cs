using Bookify.Domain.Users;
using Bookify.Shared.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Persistence;

public sealed class DatabaseUpdater(ApplicationDbContext dbContext)
{
    public async Task UpdateDatabaseAsync(CancellationToken cancellationToken = default)
    {
        if ((await dbContext.Database.GetPendingMigrationsAsync(cancellationToken: cancellationToken)).Any())
        {
            await dbContext.Database.MigrateAsync(cancellationToken: cancellationToken);
        }
        
        if (await dbContext.Database.CanConnectAsync(cancellationToken: cancellationToken))
        {
            await UpdateIdentityDataAsync(cancellationToken);
        }
    }
    
    private async Task UpdateIdentityDataAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        var existingPermissions = dbContext.Set<Permission>().ToList();
        var existingRoles = dbContext.Set<Role>().Include(r => r.Permissions).ToList();

        // Step 1: Add missing permissions
        var permissionsToAdd = Permissions.All
            .Where(p => existingPermissions.All(ep => ep.Name != p))
            .Select((permission, index) => new Permission(index + 1, permission))
            .ToList();

        if (permissionsToAdd.Count > 0)
        {
            dbContext.Set<Permission>().AddRange(permissionsToAdd);
            existingPermissions.AddRange(permissionsToAdd);
        }
        
        foreach (var roleName in Roles.Default)
        {
            var role = existingRoles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                role = new Role(id: existingRoles.Count + 1, name: roleName);
                dbContext.Set<Role>().Add(role);
                existingRoles.Add(role);
            }
            
            var requiredPermissions = roleName switch
            {
                Roles.Basic => Permissions.Basic,
                Roles.Admin => Permissions.Admin,
                _ => []
            };
            
            var currentPermissionNames = role.Permissions.Select(p => p.Name).ToHashSet();
            var permissionsToAssign = existingPermissions
                .Where(p => requiredPermissions.Contains(p.Name) && !currentPermissionNames.Contains(p.Name))
                .ToList();

            if (permissionsToAssign.Count > 0)
            {
                foreach (var permission in permissionsToAssign)
                {
                    role.Permissions.Add(permission);
                }
                dbContext.Set<Role>().Update(role);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}