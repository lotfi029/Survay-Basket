namespace Survay_Basket.API.Contracts.Polls;

public record PollRequest(
    string Title,
    string Description,
    DateTime StartsAt,
    DateTime EndsAt
    );
