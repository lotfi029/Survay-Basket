namespace Survay_Basket.API.Contracts.Authentication;

public record ConfirmEmailRequest(
    string UserId,
    string Code
);