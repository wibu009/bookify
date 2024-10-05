using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService
{
    private readonly ApplicationDbContext _dbContext;

    public AuthorizationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserRolesResponse> GetRolesForUserAsync(string identityId,
        CancellationToken cancellationToken = default)
    {
        var roles = await _dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            }).FirstAsync(cancellationToken);
        
        return roles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId, CancellationToken cancellationToken = default)
    {
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

        return permissions;
    }
}