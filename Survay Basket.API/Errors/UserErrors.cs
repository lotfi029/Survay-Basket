namespace Survay_Basket.API.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials 
        = new Error("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);

    public static readonly Error DublicatedEmail
        = new("User.DubplicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);
    
    public static readonly Error EmailIsNotConfirmed
        = new("User.EmailIsNotConfirmed", "Email Is Not Confirmed", StatusCodes.Status409Conflict);
    
    public static readonly Error InvalidCode
        = new("User.InvalidCode", "Invalid Code", StatusCodes.Status409Conflict);

    public static readonly Error DuplicatedConfirmation
        = new("User.DuplicatedConfirmation", "Duplicated Confirmation", StatusCodes.Status400BadRequest);
}
