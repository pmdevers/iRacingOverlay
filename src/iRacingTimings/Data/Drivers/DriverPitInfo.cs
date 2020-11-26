using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;

namespace iRacingTimings.Data.Drivers
{
    public class DriverPitInfo
    {
        private const float PIT_MINSPEED = 0.01f;
        private readonly Driver _driver;

        public int PitStops { get; private set; }
        public bool InPitLane { get; private set; }
        public bool InPitStall { get; private set; }

        public LapTime PitLaneEntryTime { get; private set; }
        public LapTime PitLaneExitTime { get; private set; }

        public LapTime PitStallEntryTime { get; private set; }
        public LapTime PitStallExitTime { get; private set; }

        public LapTime LastPitLaneTime { get; private set; }
        public LapTime LastPitStallTime { get; private set; }

        public LapTime CurrentPitLaneTime { get; private set; }
        public LapTime CurrentPitStallTime { get; private set; }


        public int LastPitLap { get; private set; }
        public int CurrentStint { get; private set; }

        public DriverPitInfo(Driver driver)
        {
            _driver = driver;
        }

        public void Update(Telemetry telemetry)
        {
            if (_driver.Live.TrackSurface == TrackLocation.NotInWorld)
            {
                return;
            }

            InPitLane = _driver.Live.TrackSurface == TrackLocation.AproachingPits || 
                        _driver.Live.TrackSurface == TrackLocation.InPitStall;

            InPitStall = _driver.Live.TrackSurface == TrackLocation.InPitStall;

            
        }
    }
}
