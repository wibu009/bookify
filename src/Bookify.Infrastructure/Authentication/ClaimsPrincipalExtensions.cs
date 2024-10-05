using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Bookify.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    
    public static Guid GetUserId(this ClaimsPrincipal? principal) =>
        Guid.TryParse(principal?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : throw new ApplicationException("User identity is unavailable");
    public static string GetIdentityId(this ClaimsPrincipal? principal) => 
        principal?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new ApplicationException("User identity is unavailable");
}