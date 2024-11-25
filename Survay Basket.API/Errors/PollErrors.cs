namespace Survay_Basket.API.Errors;

public static class PollErrors
{
    public static readonly Error NotFound
        = new Error("Poll.PollNotFound", "this poll is not found");

    public static readonly Error InvalidTitle
        = new Error("Poll.InvalidPollTitle", "this title exit before please select other title");



}
