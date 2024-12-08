using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);
// Response Caching 
//services.AddResponseCaching();

// Output Caching
//services.AddOutputCache();
//services.AddOutputCache(options =>
//{
//    options.AddPolicy("Polls", e => e.Cache().Expire(TimeSpan.FromSeconds(120)).Tag("availableQuestions"));
//});

// Memory Cach
//services.AddMemoryCache();

// Distributed Cach
//services.AddDistributedMemoryCache();
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();


app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseResponseCaching();
//app.UseOutputCache();


app.UseRateLimiter();


app.MapControllers();

app.UseExceptionHandler();

app.Run();
