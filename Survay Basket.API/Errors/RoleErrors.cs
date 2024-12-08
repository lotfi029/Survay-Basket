namespace Survay_Basket.API.Errors;

public class RoleErrors
{
    public static readonly Error RoleNotFound
        = new("Role.NotFoundRole", "Not Found Role", StatusCodes.Status404NotFound);

    public static readonly Error DublicatedRole
        = new("Role.DublicatedRole", "This Role Is Exists before", StatusCodes.Status409Conflict);

    public static readonly Error InvalidPermission
        = new("Role.InvalidPermission", "this permission not exist To be added in the permissions", StatusCodes.Status400BadRequest);
}