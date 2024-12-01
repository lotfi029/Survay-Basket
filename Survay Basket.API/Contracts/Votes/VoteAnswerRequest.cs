namespace Survay_Basket.API.Contracts.Votes;

public record VoteAnswerRequest(
    int QuestionId,
    int AnswerId
    );
