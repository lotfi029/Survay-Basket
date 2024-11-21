namespace Survay_Basket.API.Contracts.Polls;

public record PollResponse(
    int Id,
    string Title,
    string Description,
    bool IsPublished,
    string? Message,
    DateTime StartsAt,
    DateTime EndsAt
    );