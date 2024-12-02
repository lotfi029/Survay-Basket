namespace Survay_Basket.API.Contracts.Authentication;

public record ResetPasswordRequest(
    string Email,
    string Code,
    string Password
);