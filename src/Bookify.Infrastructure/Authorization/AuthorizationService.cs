using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService(
    ApplicationDbContext dbContext,
    ICacheService cacheService)
{
    public async Task<UserRolesResponse> GetRolesForUserAsync(string identityId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"auth:roles:{identityId}";
        
        var cachedRoles = await cacheService.GetAsync<UserRolesResponse>(cacheKey, cancellationToken);
        if (cachedRoles != null) return cachedRoles;
        
        var roles = await dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            }).FirstAsync(cancellationToken);
        await cacheService.SetAsync(cacheKey, roles, cancellationToken: cancellationToken);
        
        return roles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"auth:permissions:{identityId}";
        
        var cachedPermissions = await cacheService.GetAsync<HashSet<string>>(cacheKey, cancellationToken);
        if (cachedPermissions != null) return cachedPermissions;
        
        var user = await dbContext.Set<User>()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.IdentityId == identityId, cancellationToken);
        if (user == null)
        {
            return [];
        }
        var permissions = user.Roles
            .SelectMany(r => r.Permissions)
            .Select(p => p.Name)
            .ToHashSet();
        await cacheService.SetAsync(cacheKey, permissions, cancellationToken: cancellationToken);

        return permissions;
    }
}