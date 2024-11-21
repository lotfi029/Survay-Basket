namespace Survay_Basket.API.Services;

public interface IAuthService
{
    Task<AuthResponse?> GetTokenAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
