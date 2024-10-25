using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public AuthorizationService(
        ApplicationDbContext dbContext,
        ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public async Task<UserRolesResponse> GetRolesForUserAsync(string identityId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"auth:roles:{identityId}";
        
        var cachedRoles = await _cacheService.GetAsync<UserRolesResponse>(cacheKey, cancellationToken);
        if (cachedRoles != null) return cachedRoles;
        
        var roles = await _dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            }).FirstAsync(cancellationToken);
        await _cacheService.SetAsync(cacheKey, roles, cancellationToken: cancellationToken);
        
        return roles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"auth:permissions:{identityId}";
        
        var cachedPermissions = await _cacheService.GetAsync<HashSet<string>>(cacheKey, cancellationToken);
        if (cachedPermissions != null) return cachedPermissions;
        
        var user = await _dbContext.Set<User>()
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
        await _cacheService.SetAsync(cacheKey, permissions, cancellationToken: cancellationToken);

        return permissions;
    }
}