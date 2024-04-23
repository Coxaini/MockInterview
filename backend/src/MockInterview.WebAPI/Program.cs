using MockInterview.Identity.API;
using MockInterview.InterviewOrchestrator;
using MockInterview.Interviews.API;
using MockInterview.Interviews.API.Hubs;
using MockInterview.Matchmaking.API;
using MockInterview.WebAPI;
using MockInterview.WebAPI.Middlewares;
using Shared.Messaging;
using Shared.Persistence.Redis;
using Shared.Scheduler;
using Shared.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddQuartzScheduler(builder.Configuration);

builder.Services
    .AddWebApi(builder.Configuration)
    .AddIdentityModule(builder.Configuration)
    .AddMatchmakingModule(builder.Configuration)
    .AddInterviewModule(builder.Configuration)
    .AddMessaging(builder.Configuration, new[]
    {
        typeof(MockInterview.Identity.Application.DependencyInjection).Assembly,
        typeof(MockInterview.Matchmaking.Application.DependencyInjection).Assembly,
        typeof(MockInterview.Interviews.Application.DependencyInjection).Assembly,
        typeof(InterviewOrchestratorMassTransitConfiguration).Assembly
    });

builder.Services.AddRedis(builder.Configuration);

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Default",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Default");

app.UseMiddleware<AuthTokenSetterMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
if (!app.Environment.IsDevelopment()) app.UseHttpsRedirection();

app.UseIdentityModule();
app.UseInterviewModule();

app.MapHub<ConferenceHub>("conference-hub");
app.MapControllers();

app.Run();