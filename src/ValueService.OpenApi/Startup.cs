using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Core.ServiceDiscovery;

namespace ValueService.OpenApi
{
	public class Startup
	{
		private const string SERVICE_NAME = "ValueService.OpenApi";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddConsul(Configuration.GetServiceConfig());
			services.AddHttpContextAccessor();
			services.AddControllers();
			services.AddCors();
			services.AddRouting(options => options.LowercaseUrls = true);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseRouting();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}
			app.UseRouting();
			app.UseStaticFiles();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapGet("", async context =>
				{
					await context.Response.WriteAsync(SERVICE_NAME);
				});
			});
		}
	}
}
