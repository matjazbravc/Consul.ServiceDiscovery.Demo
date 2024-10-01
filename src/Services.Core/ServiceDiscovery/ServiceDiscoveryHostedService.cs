using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Services.Core.ServiceDiscovery;

public class ServiceDiscoveryHostedService(
  ILogger<ServiceDiscoveryHostedService> logger,
  IConsulClient client,
  ServiceConfig config)
  : IHostedService
{
  private AgentServiceRegistration _serviceRegistration;

  /// <summary>
  /// Registers service to Consul registry
  /// </summary>
  public async Task StartAsync(CancellationToken cancellationToken)
  {
    _serviceRegistration = new AgentServiceRegistration
    {
      ID = config.Id,
      Name = config.Name,
      Address = config.ApiUrl,
      Port = config.Port,
      Check = new AgentServiceCheck()
      {
        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
        Interval = TimeSpan.FromSeconds(15),
        HTTP = $"http://{config.ApiUrl}:{config.Port}/api/values/{config.HealthCheckEndPoint}",
        Timeout = TimeSpan.FromSeconds(5)
      }
    };

    try
    {
      await client.Agent.ServiceDeregister(_serviceRegistration.ID, cancellationToken).ConfigureAwait(false);
      await client.Agent.ServiceRegister(_serviceRegistration, cancellationToken).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, $"Error while trying to deregister in {nameof(StartAsync)}");
    }
  }

  /// <summary>
  /// If the service is shutting down it deregisters service from Consul registry
  /// </summary>
  public async Task StopAsync(CancellationToken cancellationToken)
  {
    try
    {
      await client.Agent.ServiceDeregister(_serviceRegistration.ID, cancellationToken).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, $"Error while trying to deregister in {nameof(StopAsync)}");
    }
  }
}