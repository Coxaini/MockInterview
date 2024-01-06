using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MockInterview.Interviews.Application.Abstractions.Repositories;
using MockInterview.Interviews.Domain.Entities;
using MockInterview.Interviews.Infrastructure.Persistence;
using MockInterview.Interviews.Infrastructure.Persistence.Repositories;
using MockInterview.Interviews.Infrastructure.Persistence.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace MockInterview.Interviews.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        InterviewMongoDbSettings interviewMongoDbSettings = new();
        configuration.Bind("InterviewMongoDbSettings", interviewMongoDbSettings);
        services.AddSingleton(Options.Create(interviewMongoDbSettings));

        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        var mongoClient = new MongoClient(interviewMongoDbSettings.ConnectionString);
        services.AddSingleton<IMongoClient>(mongoClient);

        services.AddSingleton<InterviewMongoDbContext>();
        services.AddSingleton<IInterviewRepository, InterviewRepository>();

        var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

        BsonClassMap.RegisterClassMap<Interview>(cm =>
        {
            cm.AutoMap();
            cm.MapProperty(x => x.FirstMemberQuestions);
            cm.MapProperty(x => x.SecondMemberQuestions);
        });
        return services;
    }
}