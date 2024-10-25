using Bookify.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Bookify.Infrastructure.Authorization;

public sealed class HasPermissionAttribute(string resource, string action) : AuthorizeAttribute(Permissions.Build(resource, action));