namespace Survay_Basket.API.Contracts.Results;

public record VoteResponse(
    string VoterName,
    DateTime VoteDate,
    IEnumerable<QuestionAnswersResponse> QuestionAnswers
);
