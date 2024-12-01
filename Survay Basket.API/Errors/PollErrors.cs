namespace Survay_Basket.API.Errors;

public static class PollErrors
{
    public static readonly Error NotFound
        = new("Poll.PollNotFound", "No Poll was found with the given Id", StatusCodes.Status404NotFound);

    public static readonly Error InvalidTitle
        = new("Poll.DuplicatedPollTitle", "Anothor poll with the same title is aleardy exist.", StatusCodes.Status409Conflict);
}
