namespace Survay_Basket.API.Errors;

public static class VoteErrors
{
    public static readonly Error InvalidQuestions
        = new("Vote.InvalidQuestions", "Invalid Questions", StatusCodes.Status409Conflict);

    public static readonly Error DuplicatedVote
        = new("Vote.DuplicatedVoted", "You Already Vote Before to this question", StatusCodes.Status409Conflict);
}
