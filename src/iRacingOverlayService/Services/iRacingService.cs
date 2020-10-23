using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iRacingOverlayService.Hubs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using iRacingSDK;
using iRacingSDK.Logging;
using Microsoft.AspNetCore.SignalR;

namespace iRacingOverlayService.Services
{
	public class iRacingService : BackgroundService
	{
		private readonly ILogger<iRacingService> _logger;
		private readonly IHubContext<StandingsHub, IStandingsHub> _standingsHub;
		private readonly iRacingConnection _iRacing = new iRacingConnection();

		public iRacingService(ILogger<iRacingService> logger, IHubContext<StandingsHub, IStandingsHub> standingsHub)
		{
			_logger = logger;
			_standingsHub = standingsHub;
		}
		
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("{name} is starting.", nameof(iRacingService));
			stoppingToken.Register(() => _logger.LogInformation("{name} is stopping..", nameof(iRacingService)));

			while (!stoppingToken.IsCancellationRequested)
			{
				var data = _iRacing.GetDataFeed().First();

				foreach (var driver in data.SessionData.DriverInfo.CompetingDrivers)
				{
					await _standingsHub.Clients.All.ShowTime(driver.UserName);
				}

				

				await Task.Delay(10, stoppingToken);
			}
				

			_logger.LogInformation("{name} has stopped.", nameof(iRacingService));
		}
	}
}
