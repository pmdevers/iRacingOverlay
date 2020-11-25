using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iRacingSDK;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace iRacingTimings.Data
{
    public interface ISubService
    {
        void Update(DataSample sample);
        void UpdateSession(DataSample sample);
    }

    public class iRacingService
    {
        private readonly List<ISubService> _services = new List<ISubService>();
        private readonly iRacingEvents _events = new iRacingEvents();
        public Action StateChanged;



        public bool IsConnected { get; set; }

        public iRacingService(IEnumerable<ISubService> services)
        {
            if(services != null)
                _services.AddRange(services);

            _events.NewData += Update;
            _events.NewSessionData += UpdateSession;
            _events.Connected += Connected;
            _events.Disconnected += Disconnected;

            _events.StartListening();
        }

        private void Disconnected()
        {
            IsConnected = false;
        }

        private void Connected()
        {
            IsConnected = true;
        }

        private void UpdateSession(DataSample obj)
        {
            _services.ForEach(s => s.UpdateSession(obj));

            StateChanged?.Invoke();
        }

        private void Update(DataSample obj)
        {
            _services.ForEach(s =>s.Update(obj));
            StateChanged?.Invoke();
        }
    }
}
