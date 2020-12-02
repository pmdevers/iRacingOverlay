using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRacingSDK.Data;
using iRacingSDK.Messaging;

namespace iRacingSDK
{
    public class Driver
    {
        public int Id { get; internal set; }
        public int CustId { get; private set; }
        public string Name { get; private set; }
        public string ShortName { get; private set; }
        public int IRating { get; private set; }
        public License License { get; private set; }
        public string DivisionName { get; private set; }
        public string ClubName { get; private set; }
        public bool IsSpectator { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Prefix { get; private set; }

        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public bool IsCurrentDriver { get; internal set; }
        public bool IsPaceCar { get; private set; }

        public DriverCarInfo Car { get; internal set; }
        public DriverLiveInfo Live { get; internal set; }
        public DriverResultsInfo Results { get; internal set; }

        public Driver()
        {
            Car = new DriverCarInfo(this);
            Live = new DriverLiveInfo(this);
            Results = new DriverResultsInfo(this);
        }

        internal static Driver FromSession(SessionData sessionData, int id)
        {
            var d = sessionData.DriverInfo.Drivers.SingleOrDefault(x => x.CarIdx == id);

            if (d == null)
            {
                return null;
            }

            var driver = new Driver() { Id = id };

            driver.UpdateStaticInfo(sessionData);
            driver.UpdateDynamicInfo(sessionData);

            return driver;
        }

        private void UpdateStaticInfo(SessionData sessionData)
        {
            var d = sessionData.DriverInfo.Drivers.SingleOrDefault(x => x.CarIdx == Id);

            TeamId = d.TeamID;
            TeamName = d.TeamName;

            Car.Update(sessionData);

            IsPaceCar = d.IsPaceCar;
        }

        internal void UpdateDynamicInfo(SessionData sessionData)
        {
            var d = sessionData.DriverInfo.Drivers.SingleOrDefault(x => x.CarIdx == Id);
            Name = d.UserName;

            var names = Name.Split(" ");

            FirstName = names.First();
            LastName = names.Last();
            Prefix = string.Join(" ", names.Skip(1).SkipLast(1));
            
            CustId = d.UserID;

            ShortName = d.AbbrevName;
            IRating = d.IRating;
            License = new License(d.LicLevel, d.LicSubLevel, d.LicColor);

            IsSpectator = d.IsSpectator == 1;

            ClubName = d.ClubName;
            DivisionName = d.DivisionName;
        }

        
    }

    public class DriverCarInfo
    {
        private readonly Driver _driver;

        public int CarClassId { get; private set; }

        public DriverCarInfo(Driver driver)
        {
            _driver = driver;
        }

        internal void Update(SessionData sessionData)
        {
            var d = sessionData.DriverInfo.Drivers.SingleOrDefault(x => x.CarIdx == _driver.Id);

            CarClassId = d.CarClassID;
        }
    }

    public class DriverLiveInfo
    {
        private readonly Driver _driver;

        public int Position { get; internal set; }
        public int ClassPosition { get; internal set; }

        public float SteeringAngle { get; private set; }
        public float Rpm { get; private set; }
        public int Gear { get; private set; }
        public TrackLocation TrackSurface { get; private set; }
        public float LapDistance { get; private set; }
        public int Lap { get; private set; }

        public float TotalLapDistance => Lap + LapDistance;
        

        public DriverLiveInfo(Driver driver)
        {
            _driver = driver;
        }

        internal void Update(Telemetry telemetry)
        {
            Lap = telemetry.CarIdxLap[_driver.Id];
            LapDistance = telemetry.CarIdxLapDistPct[_driver.Id];
            TrackSurface = telemetry.CarIdxTrackSurface[_driver.Id];

            Gear = telemetry.CarIdxGear[_driver.Id];
            Rpm = telemetry.CarIdxRPM[_driver.Id];
            SteeringAngle = telemetry.CarIdxSteer[_driver.Id];

            Position = telemetry.CarIdxPosition[_driver.Id];
        }

       
    }

    public class DriverResultsInfo
    {
        private readonly Driver _driver;
        private int _currentSessionNumber;
        private readonly Dictionary<int, DriverResult> _sessions = new Dictionary<int, DriverResult>();

        public IReadOnlyDictionary<int, DriverResult> Sessions => _sessions;

        public DriverResultsInfo(Driver driver)
        {
            _driver = driver;
        }

        public bool HasResult(int sessionNumber) => Sessions.ContainsKey(sessionNumber);

        public DriverResult this[int sessionNumber] => FromSession(sessionNumber);

        public DriverResult Current => FromSession(_currentSessionNumber);

        public DriverResult FromSession(int sessionNumber)
        {
            if (HasResult(sessionNumber)) return Sessions[sessionNumber];
            return new DriverResult(_driver, sessionNumber);
        }

        public void SetResults(int sessionNumber, SessionData sessionData)
        {
            if (!HasResult(sessionNumber))
            {
                _sessions.Add(sessionNumber, new DriverResult(_driver, sessionNumber));
            }

            _currentSessionNumber = sessionNumber;
            var results = this[sessionNumber];

            results.Update(sessionData);
        }

        public void Update(SessionData sessionData)
        {
            foreach (var session in sessionData.SessionInfo.Sessions)
            {
                SetResults(session.SessionNum, sessionData);   
            }
        }
    }

    public class DriverResult
    {
        private readonly Driver _driver;
        private readonly int _sessionNumber;
        
        public DriverResult(Driver driver, in int sessionNumber)
        {
            _driver = driver;
            _sessionNumber = sessionNumber;
            Laps = new LaptimeCollection();
        }

        public int Position { get; private set; }
        public int ClassPosition { get; private set; }

        public int LapsComplete { get; private set; }
        public double Time { get; private set; }
        public int FastestLap { get; private set; }
        public int LapsLed { get; private set; }
        public Laptime LastTime { get; private set; }
        public Laptime FastestTime { get; private set; }
        public Laptime AverageTime { get; private set; }
        public double LapsDriven { get; private set; }
        public int Lap { get; private set; }
        public string OutReason { get; private set; }
        public int OutReasonId { get; private set; }
        public int Incidents { get; private set; }

        public LaptimeCollection Laps { get; set; }
        
        public bool IsEmpty { get; private set; }

        public void Update(SessionData sessionData)
        {
            var results = sessionData.SessionInfo.Sessions[_sessionNumber]?.ResultsPositions?
                .SingleOrDefault(x=>x.CarIdx == _driver.Id);

            if(results == null) { return; }

            IsEmpty = false;
            Position = results.Position;
            ClassPosition = results.ClassPosition;

            Lap = results.Lap;
            Time = results.Time;
            FastestLap = results.FastestLap;
            FastestTime = new Laptime(results.FastestTime);
            LastTime = new Laptime(results.LastTime);
            LapsLed = results.LapsLed;

            var previousLaps = LapsComplete;

            LapsComplete = results.LapsComplete;
            LapsDriven = results.LapsDriven;

            FastestTime.LapNumber = FastestLap;
            LastTime.LapNumber = LapsComplete;

            if (LapsComplete > previousLaps)
            {
                Laps.Add(LastTime);
                AverageTime = Laps.Average();
            }

            Incidents = results.Incidents;
            OutReasonId = results.ReasonOutId;
            OutReason = results.ReasonOutStr;
        }

    }
}
