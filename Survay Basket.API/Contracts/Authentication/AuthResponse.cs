namespace Survay_Basket.API.Contracts.Authentication;

public record AuthResponse(
    string Id,
    string FirstName,
    string LastName,
    string? Email,
    string Token,
    string Type,
    int ExpiresIn,
    string RefreshToken,
    DateTime RefreshTokenExpiration
);
