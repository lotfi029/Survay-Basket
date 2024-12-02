namespace Survay_Basket.API.Contracts.Users;

public record UserProfileResponse (
    string Email,
    string FirstName,
    string LastName,
    string UserName
);