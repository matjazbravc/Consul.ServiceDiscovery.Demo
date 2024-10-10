using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using Services.Gateway.Services;
using System.IO;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args)
  .UseContentRoot(Directory.GetCurrentDirectory())
  .ConfigureWebHostDefaults(webBuilder =>
  {
    webBuilder.ConfigureServices(services =>
      services
        .AddOcelot()
        .AddConsul<MyConsulServiceBuilder>()
        .AddCacheManager(x =>
        {
          x.WithDictionaryHandle();
        })
        .AddPolly());
    webBuilder.Configure(app =>
      app.UseOcelot().Wait())
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
      config
        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddJsonFile("ocelot.json", false, true)
        .AddEnvironmentVariables();
    })
    .ConfigureLogging((builderContext, logging) =>
    {
      logging.ClearProviders();
      logging.AddConsole();
      logging.AddDebug();
    });
  });

IHost host = hostBuilder.Build();
await host.RunAsync();