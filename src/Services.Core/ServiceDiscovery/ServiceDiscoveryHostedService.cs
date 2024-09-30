using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Services.Core.ServiceDiscovery;

public class ServiceDiscoveryHostedService : IHostedService
{
  private readonly IConsulClient _client;
  private readonly ServiceConfig _config;
  private AgentServiceRegistration _registration;
  private readonly ILogger _logger;

  public ServiceDiscoveryHostedService(IConsulClient client, ServiceConfig config)
  {
    _client = client;
    _config = config;

    using ILoggerFactory loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
      .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace));

    _logger = loggerFactory.CreateLogger<ServiceDiscoveryHostedService>();
  }

  // Registers service to Consul registry
  public async Task StartAsync(CancellationToken cancellationToken)
  {
    _registration = new AgentServiceRegistration
    {
      ID = _config.Id,
      Name = _config.Name,
      Address = _config.ApiUrl,
      Port = _config.Port,
      Check = new AgentServiceCheck()
      {
        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
        Interval = TimeSpan.FromSeconds(15),
        HTTP = $"http://{_config.ApiUrl}:{_config.Port}/api/values/{_config.HealthCheckEndPoint}",
        Timeout = TimeSpan.FromSeconds(5)
      }
    };

    try
    {
      await _client.Agent.ServiceDeregister(_registration.ID, cancellationToken).ConfigureAwait(false);
      await _client.Agent.ServiceRegister(_registration, cancellationToken).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error while trying to deregister in StartAsync()");
    }
  }

  // If the service is shutting down it deregisters service from Consul registry
  public async Task StopAsync(CancellationToken cancellationToken)
  {
    try
    {
      await _client.Agent.ServiceDeregister(_registration.ID, cancellationToken).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error while trying to deregister in StopAsync()");
    }
  }
}