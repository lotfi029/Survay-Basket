using Survay_Basket.API.Contracts.Users;

namespace Survay_Basket.API.Services;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string UserId);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePasswordAsync(string UserId, ChangePasswordRequest request);
    Task<Result<UserResponse>> GetAsync(string id);
    Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(string id);
    Task<Result> UnlockAsync(string id);
}
