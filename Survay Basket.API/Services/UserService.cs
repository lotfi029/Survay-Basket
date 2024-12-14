using Serilog;
using Survay_Basket.API.Abstractions;
using Survay_Basket.API.Contracts.Roles;
using Survay_Basket.API.Contracts.Users;
using System.Threading;

namespace Survay_Basket.API.Services;

public class UserService(
    UserManager<ApplicationUser> userManager, 
    ApplicationDbContext context, 
    IRoleService roleService) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;
    private readonly IRoleService _roleService = roleService;

    public async Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.Users.AnyAsync(e => request.Email == e.Email, cancellationToken))
            return Result.Failure<UserResponse>(UserErrors.DuplicatedEmail);

        var user = request.Adapt<ApplicationUser>();
        var allowedRoles = await _roleService.GetAllAsync(cancellationToken: cancellationToken);

        if (request.Roles.Except(allowedRoles.Select(e => e.Name)).Any())
            return Result.Failure<UserResponse>(RoleErrors.RoleNotFound);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure<UserResponse>(new(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        await _userManager.AddToRolesAsync(user, request.Roles);

        var response = (user, request.Roles).Adapt<UserResponse>();

        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        if (await _userManager.Users.AnyAsync(e => request.Email == e.Email && id != e.Id, cancellationToken))
            return Result.Failure(UserErrors.DuplicatedEmail);
        
        var allowedRoles = await _roleService.GetAllAsync(cancellationToken: cancellationToken);

        if (request.Roles.Except(allowedRoles.Select(e => e.Name)).Any())
            return Result.Failure(RoleErrors.RoleNotFound);
        
        user = request.Adapt(user);
        
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        await _context.UserRoles
            .Where(e => e.UserId == id)
            .ExecuteDeleteAsync(cancellationToken);
        
        

        await _userManager.AddToRolesAsync(user, request.Roles);

        return Result.Success();
    }
    public async Task<Result> ToggleStatusAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        user.IsDisabled = !user.IsDisabled;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();

            return Result.Failure(new(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        return Result.Success();
    }
    public async Task<Result> UnlockAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        var result = await _userManager.SetLockoutEndDateAsync(user, null);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();

            return Result.Failure(new(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        return Result.Success();
    }
    public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await (
        from u in _context.Users
        join ur in _context.UserRoles
        on u.Id equals ur.UserId
        join r in _context.Roles
        on ur.RoleId equals r.Id
        into roles
        where !roles.Any(e => e.Name == DefaultRoles.User.Name)
        select new
        {
            u.Id,
            u.FirstName,
            u.LastName,
            u.Email,
            u.IsDisabled,
            Roles = roles.Select(e => e.Name).ToList()
        })
        .GroupBy(u => new { u.Id, u.FirstName, u.LastName, u.IsDisabled, u.Email })
        .Select(x => new UserResponse
        (
            x.Key.Id,
            x.Key.FirstName,
            x.Key.LastName,
            x.Key.Email,
            x.Key.IsDisabled,
            x.SelectMany(x => x.Roles).ToList()
        ))
        .ToListAsync(cancellationToken);
    public async Task<Result<UserResponse>> GetAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure<UserResponse>(RoleErrors.RoleNotFound);

        var roles = await _userManager.GetRolesAsync(user);

        var response = (user, roles).Adapt<UserResponse>();

        return Result.Success(response);
    }

    public async Task<Result<UserProfileResponse>> GetProfileAsync(string UserId)
    {
        var user = await _userManager.Users
            .Where(e => e.Id == UserId)
            .ProjectToType<UserProfileResponse>()
            .SingleAsync();

        return Result.Success(user);
    }
    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
        //var user = await _userManager.FindByIdAsync(userId);

        //user = request.Adapt(user);
        //await _userManager.UpdateAsync(user!);

        await _userManager.Users
            .Where(e => e.Id == userId)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(e => e.FirstName, request.FirstName)
                       .SetProperty(e => e.LastName, request.LastName)
            );
        return Result.Success();
    }
    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        return Result.Success();
    }
}
