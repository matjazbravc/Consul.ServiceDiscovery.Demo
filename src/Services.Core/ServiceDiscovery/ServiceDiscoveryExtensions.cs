using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Services.Core.ServiceDiscovery
{
	public static class ServiceDiscoveryExtensions
	{
		public static void AddConsul(this IServiceCollection services, ServiceConfig serviceConfig)
		{
      ArgumentNullException.ThrowIfNull(serviceConfig);

      var consulClient = new ConsulClient(config =>
			{
				config.Address = serviceConfig.DiscoveryAddress;
			});

			services.AddSingleton(serviceConfig);
			services.AddSingleton<IConsulClient, ConsulClient>(_ => consulClient);
			services.AddSingleton<IHostedService, ServiceDiscoveryHostedService>();
		}
	}
}