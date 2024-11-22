using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Survay_Basket.API.Authentication;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Survay_Basket.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
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

        services.AddDataBase(configuration);
        
        // JWT
        services.AddAuthConfig(configuration);
        
        // UOW
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
    private static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'Default Connection' not found.");
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        return services;
    }
    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtProvider, JwtProvider>();

        //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var settings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).
        AddJwtBearer(o =>
        {
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings?.Key!)),
            ValidIssuer = settings?.Issuer,
            ValidAudience = settings?.Audience,
        };});


        return services;
    }

}
