namespace Survay_Basket.API.Contracts.Results;

public record PollVotesResponse(
    string Title,
    IEnumerable<VoteResponse> Votes
);
