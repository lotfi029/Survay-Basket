namespace Survay_Basket.API.Errors;

public static class RefreshTokenErrors
{
    
    public static readonly Error InvalidToken
        = new Error("Token.InvalidToken", "This Token Is Expires", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidUserId
        = new Error("Token.InvalidUserId", "there is no user with this id", StatusCodes.Status401Unauthorized);

    public static readonly Error NoRefreshToken
        = new Error("Token.NoRefreshToken", "there is no refresh tokens", StatusCodes.Status401Unauthorized);
}
