namespace Survay_Basket.API.Contracts.Users;

public record UserResponse (
    string Id,
    string FirstName, 
    string LastName,
    string Email, 
    bool IsDisable,
    IList<string> Roles
);
