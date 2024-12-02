using Survay_Basket.API.Contracts.Polls;
using Survay_Basket.API.Contracts.Question;

namespace Survay_Basket.API.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Poll, PollResponse>()
            .Map(dest => dest.Description, src => src.Summary);

        config.NewConfig<PollRequest, Poll>()
            .Map(dest => dest.Summary, src => src.Description);

        config.NewConfig<QuestionRequest, Question>()
            .Ignore(nameof(Question.Answers));

        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email);
        
    }
}
