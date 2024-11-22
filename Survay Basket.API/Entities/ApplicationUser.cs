using Microsoft.AspNetCore.Identity;

namespace Survay_Basket.API.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
