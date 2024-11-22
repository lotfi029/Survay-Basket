namespace Survay_Basket.API.Contracts.Authentication;

public record RefreshTokenRequest(
    string Token,
    string RefreshToken
);