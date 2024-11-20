using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using System.Reflection;

namespace Survay_Basket.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        
        services.AddControllers();

        services.AddSwagger();


        // Add Mapster
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));
        

        // Add Fluent Validation
        //services.AddScoped<IValidator<CreatePollRequest>, CreatePollRequestValidator>();
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}
