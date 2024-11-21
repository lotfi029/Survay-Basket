namespace Survay_Basket.API.Contracts.Authentication;

public record LoginRequest(
    string Email,
    string Password
);
