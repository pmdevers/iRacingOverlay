using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iRacingSDK;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace iRacingTimings.Data
{
    public class iRacingService : BackgroundService
    {
        private readonly ILogger<iRacingService> _logger;
        private readonly List<IiRacingService> _services = new List<IiRacingService>();
        private readonly iRacingConnection _iRacing;

        public iRacingService(ILogger<iRacingService> logger, IEnumerable<IiRacingService> services)
        {
            _logger = logger;

            if (services != null)
            {
                _services.AddRange(services);
            }
            
            _iRacing = new iRacingConnection();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
			_logger.LogInformation("{name} is starting.", nameof(iRacingService));
            stoppingToken.Register(() => _logger.LogInformation("{name} is stopping..", nameof(iRacingService)));

            while (!stoppingToken.IsCancellationRequested)
            {
                var data = _iRacing.GetDataFeed().First();
                if (data.IsConnected)
                {
                    
                    foreach (var service in _services)
                    {
                        service.Update(data);
                    }
                }

                await Task.Delay(1000 / 60);
            }

            _logger.LogInformation("{name} has stopped.", nameof(iRacingService));
		}
    }

    public interface IiRacingService
    {
        void Update(DataSample data);
    }
}
