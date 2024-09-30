using System;

namespace Services.Core.ServiceDiscovery;

public class ServiceConfig
{
  public string Id { get; set; }

  public string Name { get; set; }

  public string ApiUrl { get; set; }

  public int Port { get; set; }

  public Uri ConsulUrl { get; set; }

  public string HealthCheckEndPoint { get; set; }
}
