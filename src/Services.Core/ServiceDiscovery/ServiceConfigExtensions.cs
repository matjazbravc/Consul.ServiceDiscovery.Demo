using Microsoft.Extensions.Configuration;
using System;

namespace Services.Core.ServiceDiscovery;

public static class ServiceConfigExtensions
{
  public static ServiceConfig GetServiceConfig(this IConfiguration configuration)
  {
    ArgumentNullException.ThrowIfNull(configuration);

    ServiceConfig serviceConfig = new()
    {
      Id = configuration.GetValue<string>("ServiceConfig:Id"),
      Name = configuration.GetValue<string>("ServiceConfig:Name"),
      ApiUrl = configuration.GetValue<string>("ServiceConfig:ApiUrl"),
      Port = configuration.GetValue<int>("ServiceConfig:Port"),
      ConsulUrl = configuration.GetValue<Uri>("ServiceConfig:ConsulUrl"),
      HealthCheckEndPoint = configuration.GetValue<string>("ServiceConfig:HealthCheckEndPoint"),
    };

    return serviceConfig;
  }
}