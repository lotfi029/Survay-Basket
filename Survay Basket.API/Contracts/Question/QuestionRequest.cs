using System.Text.Json.Serialization;

namespace Survay_Basket.API.Contracts.Question;

public record QuestionRequest (
    string Content,
    
    List<string> Answers);
