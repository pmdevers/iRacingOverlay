using System;
using System.Collections.Generic;
using System.Text;
using iRacingOverlayService.Hubs;
using iRacingOverlayService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iRacingOverlayService
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddRazorPages();
			services.AddSignalR();

			services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
				{
					builder.WithOrigins("https://example.com")
						.AllowCredentials();
				});
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			app.UseRouting();
			app.UseCors();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapHub<StandingsHub>("/standings");
			});
		}
	}
}
