namespace Survay_Basket.API.Errors;

public record UserErrors
{
    public static readonly Error InvalidCredentials 
        = new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);
    
    public static readonly Error DisabledUser 
        = new("User.DisabledUser", "Disabled User Please Contact Your Adminstrator", StatusCodes.Status401Unauthorized);
    
    public static readonly Error LockedUser
        = new("User.LockoutUser", "Locked User, Please Contact Your Adminstrator", StatusCodes.Status401Unauthorized);

    public static readonly Error NotFound
        = new("User.NotFound", "Invalid Id this user not found.", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedEmail
        = new("User.DubplicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);
    
    public static readonly Error EmailIsNotConfirmed
        = new("User.EmailIsNotConfirmed", "Email Is Not Confirmed", StatusCodes.Status409Conflict);
    
    public static readonly Error InvalidCode
        = new("User.InvalidCode", "Invalid Code", StatusCodes.Status409Conflict);

    public static readonly Error DuplicatedConfirmation
        = new("User.DuplicatedConfirmation", "Duplicated Confirmation", StatusCodes.Status400BadRequest);
}
