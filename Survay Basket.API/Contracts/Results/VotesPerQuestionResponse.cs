namespace Survay_Basket.API.Contracts.Results;

public record VotesPerQuestionResponse(
    string Question,
    IEnumerable<VotesPerAnswersResponse> Answers
);
