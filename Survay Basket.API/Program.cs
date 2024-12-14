using Hangfire;
using HangfireBasicAuthenticationFilter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.SignalR;
using Scalar.AspNetCore;
using Serilog;
using Survay_Basket.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

//app.UseHangfireDashboard("/jobs", new DashboardOptions
//{
//    Authorization = [
//        new HangfireCustomBasicAuthenticationFilter
//        {
//            User = app.Configuration.GetValue<string>("HangfireSettings:Username"),
//            Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
//        }
//    ],
//    DashboardTitle = "Survay Basket Dashboard"
//});

//var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
//using var scope = scopeFactory.CreateScope();
//var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

//RecurringJob
//    .AddOrUpdate("SendNewPollsNotification", () => notificationService.SendNewPollsNotification(null,default), Cron.Daily);


//app.MapPost("broadcast", async (string message, IHubContext<ChatHub, IChatClient> context) =>
//{
//    await context.Clients.All.ReceiveMessage(message);

//    return Results.NoContent();
//});

app.UseCors("AllowAll");

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.UseExceptionHandler();



app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHub<ChatHub>("chat-hub");

app.Run();
