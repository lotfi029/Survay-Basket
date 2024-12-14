using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Asp.Versioning;
using Survay_Basket.API.OpenApiTransformers;
using Hangfire;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Survay_Basket.API.Health;
using Microsoft.EntityFrameworkCore.Internal;

namespace Survay_Basket.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddControllers();

        services.MapstartConfiguration();

        services.FluentValidationConfig();

        services.AddDataBase(configuration);

        services.AddCasheConfig();

        // JWT
        services.AddAuthConfig(configuration);

        services.InjectService();

        // Exception Handler
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // mailSettings
        // TODO: Validate Mail Setting Properities
        services.Configure<MailSetting>(configuration.GetSection(MailSetting.SectionName));


        services.AddRateLimiterConfig();

        services.AddAuthConfig();

        services
            .AddEndpointsApiExplorer()
            .AddOpenApiConfig();

        //services.AddHangfireConfig(configuration);

        services.AddHealthCheckConfig(configuration);

        services.AddSignalR();

        services.AddCorsConfig();

        return services;
    }
    private static IServiceCollection MapstartConfiguration(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }
    private static IServiceCollection FluentValidationConfig(this IServiceCollection services)
    {
        services
          .AddFluentValidationAutoValidation()
          .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

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
    private static IServiceCollection AddCasheConfig(this IServiceCollection services)
    {
        #pragma warning disable EXTEXP0018
        services.AddHybridCache();
        #pragma warning restore EXTEXP0018

        return services;
    }
    
    private static IServiceCollection AddHealthCheckConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(name: "database", connectionString: configuration.GetConnectionString("DefaultConnection")!)
            .AddHangfire(options =>
            {
                options.MinimumAvailableServers = 1;
            })
            .AddCheck<MailProviderHealthCheck>(name: "mail service");

        return services;
    }
    
    private static IServiceCollection InjectService(this IServiceCollection services)
    {
        // UOW
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<INotificationService, NotificationService>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAutherizationPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionAutherizationHandler>();

        services.AddHttpContextAccessor();

        return services;
    }
    private static IServiceCollection AddOpenApiConfig(this IServiceCollection services)
    {

        services
            .AddOpenApi("v1", options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            }); ;
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

            rateLimiterOptions.AddPolicy(RateLimiterPolicyNames.IpLimiter, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.LocalIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 2,
                        Window = TimeSpan.FromSeconds(20)
                    }
                )
            );

            rateLimiterOptions.AddPolicy(RateLimiterPolicyNames.UserLimiter, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.GetUserId(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 2,
                        Window = TimeSpan.FromSeconds(20)
                    }
                )
            );

            rateLimiterOptions.AddConcurrencyLimiter(RateLimiterPolicyNames.Concurrency, options =>
            {
                options.PermitLimit = 1000;
                options.QueueLimit = 100;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            rateLimiterOptions.AddTokenBucketLimiter(RateLimiterPolicyNames.TokenBucket, options =>
            {
                options.TokenLimit = 2;
                options.QueueLimit = 1;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
                options.TokensPerPeriod = 2;
            });

            rateLimiterOptions.AddFixedWindowLimiter(RateLimiterPolicyNames.FixedWidnow, options =>
            {
                options.PermitLimit = 2;
                options.QueueLimit = 1;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.Window = TimeSpan.FromSeconds(30);
            });

            rateLimiterOptions.AddSlidingWindowLimiter(RateLimiterPolicyNames.SlidingWindow, options =>
            {
                options.PermitLimit = 2;
                options.QueueLimit = 1;
                options.SegmentsPerWindow = 2;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.Window = TimeSpan.FromSeconds(30);
            });
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
    private static IServiceCollection AddHangfireConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
          .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        return services;
    }

    private static IServiceCollection AddCorsConfig(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            //options.AddPolicy("AllowAll", builder =>
            //{
            //    builder.AllowAnyMethod();
            //    builder.AllowAnyHeader();
            //    builder.AllowAnyOrigin();
            //    //builder.WithOrigins([""]);
            //});

            options.AddDefaultPolicy(builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()
            );
        });
        return services;
    }
}
