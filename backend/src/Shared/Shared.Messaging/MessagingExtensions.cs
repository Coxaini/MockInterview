using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Messaging.Settings;

namespace Shared.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration,
        Assembly[] consumerAssemblies)
    {
        var brokerSettings = new MessageBrokerSettings();
        configuration.Bind("MessageBroker", brokerSettings);

        services.AddSingleton(Options.Create(brokerSettings));

        services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();

            configurator.AddConsumers(consumerAssemblies);

            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(brokerSettings.Host), h =>
                {
                    h.Username(brokerSettings.Username);
                    h.Password(brokerSettings.Password);
                });
                cfg.AutoStart = true;
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddTransient<IEventBus, EventBus>();

        return services;
    }
}