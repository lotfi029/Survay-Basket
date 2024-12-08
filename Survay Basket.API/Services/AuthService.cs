using Azure.Core;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MimeKit;
using Survay_Basket.API.Abstractions.Consts;
using Survay_Basket.API.Authentication;
using Survay_Basket.API.Errors;
using Survay_Basket.API.Helpers;
using Survay_Basket.API.Settings;
using System.Data;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Survay_Basket.API.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider,
    SignInManager<ApplicationUser> signInManager,
    IEmailSender emailSender,
    IHttpContextAccessor contextAccessor,
    IOptions<MailSetting> mailSettings,
    ApplicationDbContext context,
    ILogger<AuthService> logger) : IAuthService
{
    
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IHttpContextAccessor contextAccessor = contextAccessor;
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly MailSetting _mailSettings = mailSettings.Value;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _refreshTokenExpiryDays = 14;


    public async Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        

        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);

        if (!result.Succeeded)
        {

            return Result.Failure<AuthResponse>(
                result.IsNotAllowed 
                ? UserErrors.EmailIsNotConfirmed 
                : result.IsLockedOut
                ? UserErrors.LockedUser
                : UserErrors.InvalidCredentials);
        }
        var (roles, permission) = await GetUserRolesAndClaims(user, cancellationToken);
        var (token, expiresIn) = _jwtProvider.GenerateToken(user, roles, permission);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration,
        });

        await _userManager.UpdateAsync(user);

        var respone = new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email, token, "Bearer", expiresIn, refreshToken, refreshTokenExpiration);
        
        return Result.Success(respone);
    }

    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        
        if (userId is null)
            return Result.Failure<AuthResponse>(RefreshTokenErrors.InvalidToken);

        if (await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure<AuthResponse>(RefreshTokenErrors.InvalidUserId);

        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(e => e.Token == refreshToken && e.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(RefreshTokenErrors.NoRefreshToken);

        userRefreshToken.RevokeOn = DateTime.UtcNow;

        var (roles, permission) = await GetUserRolesAndClaims(user, cancellationToken);
        var (newToken, expiresIn) = _jwtProvider.GenerateToken(user, roles, permission);

        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration,
        });

        await _userManager.UpdateAsync(user);

        var respone = new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email, newToken, "Bearer", expiresIn, newRefreshToken, refreshTokenExpiration);

        return Result.Success(respone);
    }
    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        
        if (userId is null) 
            return Result.Failure(RefreshTokenErrors.InvalidToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) 
            return Result.Failure(RefreshTokenErrors.InvalidUserId);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(e => e.Token == refreshToken && e.IsActive);
        
        if (userRefreshToken is null) 
            return Result.Failure(RefreshTokenErrors.NoRefreshToken);

        userRefreshToken.RevokeOn = DateTime.UtcNow;
        
        await _userManager.UpdateAsync(user);

        return Result.Success();
    }
    //public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    //{
    //    if (!await _userManager.Users.AnyAsync(e => e.Email == request.Email, cancellationToken))
    //        return Result.Failure<AuthResponse>(UserErrors.DublicatedEmail);

    //    var user = request.Adapt<ApplicationUser>();

    //    var result = await _userManager.CreateAsync(user, request.Password);

    //    if (!result.Succeeded)
    //    {
    //        var error = result.Errors.FirstOrDefault();

    //        return Result.Failure<AuthResponse>(new Error(error!.Code, error.Description, StatusCodes.Status400BadRequest));
    //    }

    //var(roles, permission) = await GetUserRolesAndClaims(user, cancellationToken);
    //var(newToken, expiresIn) = _jwtProvider.GenerateToken(user, roles, permission);

    //    var refreshToken = GenerateRefreshToken();
    //    var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

    //    user.RefreshTokens.Add(new RefreshToken
    //    {
    //        Token = refreshToken,
    //        ExpiresOn = refreshTokenExpiration,
    //    });

    //    await _userManager.UpdateAsync(user);

    //    var respone = new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email, token, "Bearer", expiresIn, refreshToken, refreshTokenExpiration);

    //    return Result.Success(respone);

    //}
    
    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.Users.AnyAsync(e => e.Email == request.Email, cancellationToken))
            return Result.Failure(UserErrors.DuplicatedEmail);


        var user = request.Adapt<ApplicationUser>();

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var error = result.Errors.FirstOrDefault();
            return Result.Failure(new Error(error!.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // TODO: Send Email
        _logger.LogInformation("Confirmation Email Code: {code}", code);

        return Result.Success();
    }

    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {

        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var code = request.Code;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch(FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (!result.Succeeded)
        {
            var error = result.Errors.FirstOrDefault();
            return Result.Failure(new Error(error!.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        await _userManager.AddToRoleAsync(user, DefaultRoles.User);
        return Result.Success();
        //return Result.Success();
        // TODO: Send Email
    }
    public async Task<Result> ReConfirmAsync(ResendConfirmationEmailRequest request)
    {

        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("ReConfirmation Email: {code}", code);

        return Result.Success();
        // TODO: Send Email
    }

    public async Task<Result> SendResetPasswordCodeAsync(ForgetPasswordRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailIsNotConfirmed);

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("Confirmation Code: {code}", code);

        return Result.Success();
    }
    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);

        var code = request.Code;
        IdentityResult result;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            if (await _userManager.CheckPasswordAsync(user, request.Password))
                return Result.Failure(new Error("User.InvalidPassword", "this password is used before Select another one.", StatusCodes.Status409Conflict));
            result = await _userManager.ResetPasswordAsync(user, code, request.Password);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }

        if (!result.Succeeded)
        {
            var error = result.Errors.FirstOrDefault();
            return Result.Failure(new Error(error!.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        return Result.Success();
    }
    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
    
    private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndClaims(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var permission = await (
            from r in _context.Roles
            join c in _context.RoleClaims
            on r.Id equals c.RoleId
            where roles.Contains(r.Name!)
            select c.ClaimValue)
            .Distinct()
            .ToListAsync(cancellationToken);

        return (roles, permission);
    }
}
