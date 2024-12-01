using Microsoft.AspNetCore.Identity;

using Survay_Basket.API.Authentication;
using Survay_Basket.API.Errors;
using System.Security.Cryptography;

namespace Survay_Basket.API.Services;

public class AuthService(UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider) : IAuthService
{
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly int _refreshTokenExpiryDays = 14;


    public async Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password)) 
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var (token, expiresIn) = _jwtProvider.GenerateToken(user);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration,
        });

        await _userManager.UpdateAsync(user);

        var respone = new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email, token, "Bearer", expiresIn, refreshToken, refreshTokenExpiration);
        
        return Result.Success(respone);
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        
        if (userId is null)
            return Result.Failure<AuthResponse>(RefreshTokenErrors.InvalidToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(RefreshTokenErrors.InvalidUserId);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(e => e.Token == refreshToken && e.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(RefreshTokenErrors.NoRefreshToken);

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

        var respone = new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email, newToken, "Bearer", expiresIn, newRefreshToken, refreshTokenExpiration);

        return Result.Success(respone);
    }
    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        
        if (userId is null) 
            return Result.Failure(RefreshTokenErrors.InvalidToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) 
            return Result.Failure(RefreshTokenErrors.InvalidUserId);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(e => e.Token == refreshToken && e.IsActive);
        
        if (userRefreshToken is null) 
            return Result.Failure(RefreshTokenErrors.NoRefreshToken);

        userRefreshToken.RevokeOn = DateTime.UtcNow;
        
        await _userManager.UpdateAsync(user);

        return Result.Success();
    }
}
