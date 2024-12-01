using System.Security.Claims;

namespace Survay_Basket.API.Extensions;

public static class UserExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user) => 
        user.FindFirstValue(ClaimTypes.NameIdentifier);
}
