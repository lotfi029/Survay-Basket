using Microsoft.AspNetCore.Identity;
using Survay_Basket.API.Authentication;

namespace Survay_Basket.API.Services;

public class AuthService(UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider) : IAuthService
{
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<AuthResponse?> GetTokenAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null) 
            return null;

        string password = request.Password;

        var validPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!validPassword) return null;


        var (token, expiresIn) = _jwtProvider.GenerateToken(user);


        return new(user.Id, user.FirstName, user.LastName, user.Email, token, "Bearer", expiresIn);
    }

}
