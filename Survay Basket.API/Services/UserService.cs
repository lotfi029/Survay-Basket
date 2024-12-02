﻿using Survay_Basket.API.Contracts.Users;

namespace Survay_Basket.API.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

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
