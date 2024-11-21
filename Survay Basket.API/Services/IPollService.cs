using Survay_Basket.API.Contracts.Polls;

namespace Survay_Basket.API.Services;

public interface IPollService
{
    Task<PollResponse> AddAsync(PollRequest request, CancellationToken cancellationToken = default);
    Task<PollResponse> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PollResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);
}
