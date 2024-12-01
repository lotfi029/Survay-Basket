using Survay_Basket.API.Contracts.Results;

namespace Survay_Basket.API.Services;

public interface IResultService
{
    Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotePerDayResponse(int pollId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotePerQuestionResponse(int pollId, CancellationToken cancellationToken = default);
}
