using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Services.Core.ServiceDiscovery;

public static class ServiceDiscoveryExtensions
{
  public static void AddConsul(this IServiceCollection services, ServiceConfig serviceConfig)
  {
    ArgumentNullException.ThrowIfNull(serviceConfig);

    var consulClient = new ConsulClient(config =>
      {
        config.Address = serviceConfig.ConsulUrl;
      });

    services.AddSingleton(serviceConfig);
    services.AddSingleton<IConsulClient, ConsulClient>(provider => consulClient);
    services.AddSingleton<IHostedService, ServiceDiscoveryHostedService>();
  }
}