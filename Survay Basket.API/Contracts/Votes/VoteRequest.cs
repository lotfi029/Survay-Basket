namespace Survay_Basket.API.Contracts.Votes;

public record VoteRequest(
    IEnumerable<VoteAnswerRequest> Answers
    );
