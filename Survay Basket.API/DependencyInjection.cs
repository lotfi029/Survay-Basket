using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Survay_Basket.API.Authentication;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Survay_Basket.API.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Survay_Basket.API.Authentication.Filters;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Asp.Versioning;

namespace Survay_Basket.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddControllers();
        //services.AddSwagger();

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

        // Distributed
        services.AddDistributedMemoryCache();
        // Hybrid
        //services.AddHybridCache();

        // JWT
        services.AddAuthConfig(configuration);
        
        // UOW
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IEmailSender, EmailService>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAutherizationPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionAutherizationHandler>();

        services.AddHttpContextAccessor();

        // Exception Handler
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();


        // mailSettings
        services.Configure<MailSetting>(configuration.GetSection(MailSetting.SectionName));


        services.AddRateLimiterConfig();

        services.AddAuthConfig();

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
            options.UseSqlServer(connectionString, e => e.MigrationsAssembly(typeof(ApplicationDbContext).Assembly));
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

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
            {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings!.Key)),
                ValidIssuer = settings.Issuer,
                ValidAudience = settings.Audience,
            };    
        });
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        });

        return services;
    }
    private static IServiceCollection AddRateLimiterConfig(this IServiceCollection services)
    {



        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            //rateLimiterOptions.AddPolicy(RateLimiterPolicyNames.IpLimiter, httpContext =>
            //    RateLimitPartition.GetFixedWindowLimiter(
            //        partitionKey: httpContext.Connection.LocalIpAddress?.ToString(),
            //        factory: _ => new FixedWindowRateLimiterOptions
            //        {
            //            PermitLimit = 2,
            //            Window = TimeSpan.FromSeconds(20)
            //        }
            //    )
            //);

            //rateLimiterOptions.AddPolicy(RateLimiterPolicyNames.UserLimiter, httpContext =>
            //    RateLimitPartition.GetFixedWindowLimiter(
            //        partitionKey: httpContext.User.GetUserId(),
            //        factory: _ => new FixedWindowRateLimiterOptions
            //        {
            //            PermitLimit = 2,
            //            Window = TimeSpan.FromSeconds(20)
            //        }
            //    )
            //);

            rateLimiterOptions.AddConcurrencyLimiter(RateLimiterPolicyNames.Concurrency, options =>
            {
                options.PermitLimit = 1000;
                options.QueueLimit = 100;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            //rateLimiterOptions.AddTokenBucketLimiter(RateLimiterPolicyNames.TokenBucket, options =>
            //{
            //    options.TokenLimit = 2;
            //    options.QueueLimit = 1;
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    options.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
            //    options.TokensPerPeriod = 2;
            //});

            //rateLimiterOptions.AddFixedWindowLimiter(RateLimiterPolicyNames.FixedWidnow, options =>
            //{
            //    options.PermitLimit = 2;
            //    options.QueueLimit = 1;
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    options.Window = TimeSpan.FromSeconds(30);
            //});

            //rateLimiterOptions.AddSlidingWindowLimiter(RateLimiterPolicyNames.SlidingWindow, options =>
            //{
            //    options.PermitLimit = 2;
            //    options.QueueLimit = 1;
            //    options.SegmentsPerWindow = 2;
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    options.Window = TimeSpan.FromSeconds(30);
            //});
        });


        return services;
    }
    private static IServiceCollection AddAuthConfig(this IServiceCollection services)
    {

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;

            options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");

        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

}
