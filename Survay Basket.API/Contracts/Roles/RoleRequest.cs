namespace Survay_Basket.API.Contracts.Roles;

public record RoleRequest(
    string Name,
    IList<string> Permission 
);
