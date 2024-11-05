using Consul;
using Microsoft.AspNetCore.Http;
using Ocelot.Logging;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Consul.Interfaces;

namespace Services.Gateway.Services;

// MORE INFO: https://ocelot.readthedocs.io/en/latest/features/servicediscovery.html#consul-service-builder
public class MyConsulServiceBuilder(IHttpContextAccessor contextAccessor, IConsulClientFactory clientFactory, IOcelotLoggerFactory loggerFactory)
  : DefaultConsulServiceBuilder(contextAccessor, clientFactory, loggerFactory)
{
  // I want to use the agent service IP address as the downstream hostname
  protected override string GetDownstreamHost(ServiceEntry entry, Node node) => entry.Service.Address;
}