using Survay_Basket.API.Contracts.Votes;

namespace Survay_Basket.API.Services;

public interface IVoteService
{
    Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken);
}
