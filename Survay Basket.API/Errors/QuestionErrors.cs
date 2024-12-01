namespace Survay_Basket.API.Errors;

public static class QuestionErrors
{
    public static readonly Error NotFound
        = new("Question.QuestionNotFound", "No Question was found with the given Id", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedContent
        = new("Question.DuplicatedQuestionTitle", "Anothor Question with the same content is aleardy exist.", StatusCodes.Status409Conflict);
}
