namespace Survay_Basket.API.Contracts.Results;

public record VotesPerDayResponse(
    DateOnly Date,
    int NumberOfVotes
);
