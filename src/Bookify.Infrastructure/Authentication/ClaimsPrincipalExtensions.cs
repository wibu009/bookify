using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Bookify.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    
    public static Guid GetUserId(this ClaimsPrincipal? principal) =>
        Guid.Parse(principal?.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? throw new ApplicationException("User identity is unavailable"));
    public static string GetIdentityId(this ClaimsPrincipal? principal) => 
        principal?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new ApplicationException("User identity is unavailable");
}