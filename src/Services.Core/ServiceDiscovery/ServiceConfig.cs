using System;

namespace Services.Core.ServiceDiscovery
{
	public class ServiceConfig
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string Address { get; set; }

		public int Port { get; set; }

		public Uri DiscoveryAddress { get; set; }

		public string HealthCheckEndPoint { get; set; }
	}
}
