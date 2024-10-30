using Microsoft.AspNetCore.Authorization;

namespace Bookify.Infrastructure.Authorization;

internal sealed class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}