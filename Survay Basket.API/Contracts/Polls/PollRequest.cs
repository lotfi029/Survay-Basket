namespace Survay_Basket.API.Contracts.Polls;

public record PollRequest(
    string Title,
    string Description,
    bool IsPublished,
    DateTime StartsAt,
    DateTime EndsAt
    );
