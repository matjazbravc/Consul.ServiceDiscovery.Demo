using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Services.Core.ServiceDiscovery;
using System;
using System.IO;
using System.Reflection;

namespace ValueService.OpenApi;

public class Startup(IConfiguration configuration)
{
  public IConfiguration Configuration { get; } = configuration;

  public void ConfigureServices(IServiceCollection services)
  {
    services.AddConsul(Configuration.GetServiceConfig());
    services.AddHttpContextAccessor();
    services.AddControllers();
    services.AddCors();
    services.AddRouting(options => options.LowercaseUrls = true);
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
      options.SwaggerDoc("v1", new OpenApiInfo
      {
        Version = "v1",
        Title = "ValueService.OpenAPI",
        Description = "An ASP.NET Core Web API for demonstrating Consul service discovery.",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
          Name = "Example Contact",
          Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
          Name = "Example License",
          Url = new Uri("https://example.com/license")
        }
      });

      string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });
  }

  public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    if (env.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
      {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
      });
    }

    // Configure the HTTP request pipeline.
    app.UseRouting();
    app.UseStaticFiles();
    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
      endpoints.MapGet("", async context => await context.Response.WriteAsync("ValueService.OpenApi"));
    });
  }
}