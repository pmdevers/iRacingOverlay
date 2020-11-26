using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;

namespace iRacingTimings.Data.Drivers
{
    public class DriverResults
    {
        private readonly Driver _driver;
        private readonly Dictionary<int, DriverSessionResults> _sessions;
        
        public DriverResults(Driver driver)
        {
            _driver = driver;
            _sessions = new Dictionary<int, DriverSessionResults>();
        }

        public bool HasResult(int sessionNumber)
        {
            return _sessions.ContainsKey(sessionNumber);
        }

        public void Update(Telemetry telemetry)
        {
            if (!HasResult(Simulator.Instance.CurrentSession))
            {
                _sessions.Add(Simulator.Instance.CurrentSession, new DriverSessionResults(_driver, Simulator.Instance.CurrentSession));
            }

            _sessions[Simulator.Instance.CurrentSession].Update(telemetry);
        }
    }
}
