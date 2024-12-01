using Survay_Basket.API.Contracts.Answers;

namespace Survay_Basket.API.Contracts.Question;

public record QuestionResponse(
    int Id, 
    string Content, 
    IEnumerable<AnswerResponse> Answers
);