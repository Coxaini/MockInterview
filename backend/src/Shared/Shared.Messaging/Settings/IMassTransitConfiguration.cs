using MassTransit;

namespace Shared.Messaging.Settings;

/// <summary>
///     Implement this interface to configure MassTransit in modules
/// </summary>
public interface IMassTransitConfiguration
{
    void ConfigureMassTransit(IBusRegistrationConfigurator configurator);
}