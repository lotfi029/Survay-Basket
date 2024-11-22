using Microsoft.AspNetCore.Identity;
using Survay_Basket.API.Authentication;
using System.Security.Cryptography;

namespace Survay_Basket.API.Services;

public class AuthService(UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider) : IAuthService
{
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    private readonly UserManager<ApplicationUser> _userManager = userManager;

    private readonly int _refreshTokenExpiryDays = 14;
    public async Task<AuthResponse?> GetTokenAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password)) 
            return null;

        var (token, expiresIn) = _jwtProvider.GenerateToken(user);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration,
        });

        await _userManager.UpdateAsync(user);

        return new(user.Id, user.FirstName, user.LastName, user.Email, token, "Bearer", expiresIn, refreshToken, refreshTokenExpiration);
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        
        if (userId is null)
            return null;

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return null;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(e => e.Token == refreshToken && e.IsActive);

        if (userRefreshToken is null)
            return null;

        userRefreshToken.RevokeOn = DateTime.UtcNow;

        var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);

        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration,
        });


        await _userManager.UpdateAsync(user);


        return new(user.Id, user.FirstName, user.LastName, user.Email, newToken, "Bearer", expiresIn, newRefreshToken, refreshTokenExpiration);
    }
    public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        
        if (userId is null) 
            return false;

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) 
            return false;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(e => e.Token == refreshToken && e.IsActive);
        
        if (userRefreshToken is null) 
            return false;

        userRefreshToken.RevokeOn = DateTime.UtcNow;
        
        await _userManager.UpdateAsync(user);

        return true;
    }
}
