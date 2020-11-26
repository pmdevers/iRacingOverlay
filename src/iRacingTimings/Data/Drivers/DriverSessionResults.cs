using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;

namespace iRacingTimings.Data.Drivers
{
    public class DriverSessionResults
    {
        private readonly Driver _driver;

        public int SessionNumber { get; }
        public List<Sector> SectorTimes { get; private set;  }
        public List<Sector> FakeSectorTimes { get; }

        public DriverSessionResults(Driver driver, int sessionNumber)
        {
            _driver = driver;
            SessionNumber = sessionNumber;


            FakeSectorTimes = new List<Sector>
            {
                new Sector() {Number = 0, StartPercentage = 0f},
                new Sector() {Number = 1, StartPercentage = 0.333f},
                new Sector() {Number = 2, StartPercentage = 0.666f}
            };
        }

        public void Update(Telemetry telemetry)
        {
            UpdateSectorTimes(telemetry);
        }

        private double _prevPos;
        private void UpdateSectorTimes(Telemetry telemetry)
        {
            var track = Simulator.Instance.SessionInfo.Track;

            if(track == null)return;
            if(!track.Sectors.Any()) return;

            var sectorcount = track.Sectors.Count;

            if (SectorTimes == null || !SectorTimes.Any())
            {
                SectorTimes = track.Sectors.Select(s => s.Copy()).ToList();
            }

            var p0 = _prevPos;
            var p1 = telemetry.CarIdxLapDistPct[_driver.Id];

            var dp = p1 - p0;

            if (p1 < -0.5)
            {
                return;
            }

            var t = (Time) telemetry.SessionTime;

            if (p0 - p1 > 0.5)
            {
                _driver.Live.CurrentSector = 0;
                _driver.Live.CurrentFakeSector = 0;
                p0 -= 1;
            }

            foreach (var s in SectorTimes)
            {
                if (p1 > s.StartPercentage && p0 < s.StartPercentage)
                {
                    var crossTime = (t - (p1 - s.StartPercentage) * dp);

                    var prevNum = s.Number <= 0 ? sectorcount - 1 : s.Number - 1;
                    var sector = SectorTimes[prevNum];
                    if(sector != null && sector.EnterSessionTime > 0)
                    {
                        sector.SectorTime = crossTime - sector.EnterSessionTime;
                    }

                    s.EnterSessionTime = crossTime;

                    _driver.Live.CurrentSector = s.Number;

                    break;
                }
            }

            sectorcount = 3;

            foreach (var s in FakeSectorTimes)
            {
                if (p1 > s.StartPercentage && p0 <= s.StartPercentage)
                {
                    // Crossed into new sector
                    var crossTime = (float)(t - (p1 - s.StartPercentage) * dp);

                    // Finish previous
                    var prevNum = s.Number <= 0 ? sectorcount - 1 : s.Number - 1;
                    var sector = FakeSectorTimes[prevNum];
                    if (sector != null && sector.EnterSessionTime > 0)
                    {
                        sector.SectorTime = crossTime - sector.EnterSessionTime;
                    }

                    // Begin next sector
                    s.EnterSessionTime = crossTime;

                    _driver.Live.CurrentFakeSector = s.Number;

                    break;
                }
            }

            _prevPos = p1;
        }
    }
}
