using MockInterview.Identity.API;
using MockInterview.InterviewOrchestrator;
using MockInterview.Interviews.API;
using MockInterview.Matchmaking.API;
using MockInterview.WebAPI;
using MockInterview.WebAPI.Middlewares;
using Shared.Messaging;
using Shared.Scheduler;
using Shared.Security;
using DependencyInjection = MockInterview.Identity.Application.DependencyInjection;

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
        typeof(DependencyInjection).Assembly,
        typeof(MockInterview.Matchmaking.Application.DependencyInjection).Assembly,
        typeof(MockInterview.Interviews.Application.DependencyInjection).Assembly,
        typeof(InterviewOrchestratorMassTransitConfiguration).Assembly
    });

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

app.MapControllers();

app.Run();