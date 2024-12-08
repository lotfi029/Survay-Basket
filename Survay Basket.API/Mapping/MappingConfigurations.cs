using Survay_Basket.API.Contracts.Polls;
using Survay_Basket.API.Contracts.Question;
using Survay_Basket.API.Contracts.Users;

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

        config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
            .Map(dest => dest, src => src.user)
            .Map(dest => dest.Roles, src => src.roles);

        config.NewConfig<CreateUserRequest, ApplicationUser>()
            .Map(dest => dest.EmailConfirmed, src => true)
            .Map(dest => dest.UserName, src => src.Email);
        
        config.NewConfig<UpdateUserRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email)
            .Map(dest => dest.NormalizedUserName, src => src.Email.ToUpper());
    }
}
