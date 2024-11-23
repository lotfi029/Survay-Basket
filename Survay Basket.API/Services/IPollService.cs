using Survay_Basket.API.Contracts.Polls;

namespace Survay_Basket.API.Services;

public interface IPollService
{
    Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken = default);
    Task<Result<PollResponse>> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);
}
