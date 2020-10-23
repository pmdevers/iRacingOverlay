using System.Threading.Tasks;
using iRacingOverlayService.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iRacingOverlayService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
	            .UseWindowsService()
	            .ConfigureServices(config =>
	            {
		            config.AddHostedService<iRacingService>();
				})
	            .ConfigureWebHostDefaults(webbuilder =>
	            {
		            webbuilder.UseStartup<Startup>();
	            });
    }
}
