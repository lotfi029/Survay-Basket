namespace Survay_Basket.API.Services;

public interface IAuthService
{
    Task<AuthResponse?> GetTokenAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
}
