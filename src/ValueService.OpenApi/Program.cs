using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ValueService.OpenApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config
						.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
						.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
						.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true)
						.AddEnvironmentVariables();
				})
				.ConfigureLogging((builderContext, logging) =>
				{
					logging.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
					logging.AddConsole();
					logging.AddDebug();
					logging.AddEventSourceLogger();
				});
	}
}
