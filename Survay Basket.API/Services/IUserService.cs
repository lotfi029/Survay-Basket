using Survay_Basket.API.Contracts.Users;

namespace Survay_Basket.API.Services;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string UserId);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePasswordAsync(string UserId, ChangePasswordRequest request);
}
