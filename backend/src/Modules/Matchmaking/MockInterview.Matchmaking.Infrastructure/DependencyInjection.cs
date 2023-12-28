using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MockInterview.Matchmaking.Application.Abstractions.Repositories;
using MockInterview.Matchmaking.Infrastructure.Repositories;
using Shared.Persistence.Neo4j;

namespace MockInterview.Matchmaking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddNeo4JDb(configuration);
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<IInterviewOrderRepository, InterviewOrderRepository>();
        services.AddScoped<IInterviewTimeSlotsRepository, InterviewTimeSlotsRepository>();
        return services;
    }
}