namespace Survay_Basket.API.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials 
        = new Error("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);
}
