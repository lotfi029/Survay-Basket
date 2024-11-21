using Survay_Basket.API.Contracts.Polls;

namespace Survay_Basket.API.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Poll, PollResponse>()
            .Map(dest => dest.Description, src => src.Summary);

        config.NewConfig<PollRequest, Poll>()
            .Map(dest => dest.Summary, src => src.Description);

        
    }
}
