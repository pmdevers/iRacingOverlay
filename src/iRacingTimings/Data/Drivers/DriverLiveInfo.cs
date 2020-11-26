using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;

namespace iRacingTimings.Data.Drivers
{
    public class DriverLiveInfo
    {
        private const float SPEED_CALC_INTERVAL = 0.5f;

        private Driver _driver;

        public int Position { get; set; }
        public int ClassPosition { get; set; }
        public int Lap { get; private set; }
        public float LapDistance { get; private set; }

        public float TotalLapDistance => Lap + LapDistance;

        public TrackLocation TrackSurface { get; private set; }

        public int Gear { get; private set; }
        public float Rpm { get; private set; }
        public double SteeringAngle { get; private set; }

        public Speed Speed { get; private set; }
        
        public string DeltaToLeader { get; set; }
        public string DeltaToNext { get; set; }

        public int CurrentSector { get; set; }
        public int CurrentFakeSector { get; set; }


        public DriverLiveInfo(Driver driver)
        {
            _driver = driver;
        }

        public void Update(Telemetry telemetry)
        {
            Lap = telemetry.CarIdxLap[_driver.Id];
            LapDistance = telemetry.CarIdxLapDistPct[_driver.Id] * 100;
            TrackSurface = telemetry.CarIdxTrackSurface[_driver.Id];

            Gear = telemetry.CarIdxGear[_driver.Id];
            Rpm = telemetry.CarIdxRPM[_driver.Id];
            SteeringAngle = telemetry.CarIdxSteer[_driver.Id];

            Position = telemetry.Positions[_driver.Id];
            ClassPosition = telemetry.CarIdxClassPosition[_driver.Id];

            CalculateSpeed(telemetry);
        }

        private Time _prevSpeedUpdateTime;
        private double _prevSpeedUpdateDist;

        private void CalculateSpeed(Telemetry telemetry)
        {
            var trackLength = (Distance) telemetry.SessionData.WeekendInfo.TrackLength;

            var t1 = (Time) telemetry.SessionTime;
            var t0 = _prevSpeedUpdateTime;

            var time = t1 - t0;

            if (time < SPEED_CALC_INTERVAL)
            {
                return;
            }

            var p1 = telemetry.CarIdxLapDistPct[_driver.Id];
            var p0 = _prevSpeedUpdateDist;

            if (p1 < -0.5 || _driver.Live.TrackSurface == TrackLocation.NotInWorld)
            {
                return;
            }

            if (p0 - p1 > 0.5)
            {
                p1 += 1;
            }

            var distancePct = p1 - p0;
            var distance = Distance.Create(trackLength * distancePct);

            if (time >= double.Epsilon)
            {
                Speed = distance / (double)time;
            }
            else
            {
                Speed = distance < 0 ? double.NegativeInfinity : double.PositiveInfinity;
            }

            _prevSpeedUpdateTime = t1;
            _prevSpeedUpdateDist = p1;
        }

    }
}
