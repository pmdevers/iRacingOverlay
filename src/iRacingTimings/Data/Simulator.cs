using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;
using iRacingTimings.Data.Drivers;

namespace iRacingTimings.Data
{
    public class Simulator
    {
        private static Simulator _instance;
        public static Simulator Instance => _instance ??= new Simulator();

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler StaticInfoChanged;
        public event EventHandler<SessionData> SessionInfoUpdated;
        public event EventHandler<Telemetry> TelemetryUpdated;
        public event EventHandler SimulationUpdated;
        public event EventHandler<RaceEvent> RaceEvent;

        private Simulator()
        {
            iRacing = new iRacingEvents();
            Drivers = new List<Driver>();
            SessionInfo = new SessionInfo();

            _mustReloadDrivers = true;
            iRacing.Connected += IRacingOnConnected;
            iRacing.Disconnected += IRacingOnDisconnected;
            iRacing.NewData += IRacingOnNewData;
            iRacing.NewSessionData += IRacingOnNewSessionData;
        }
        
        public iRacingEvents iRacing { get; private set; }
        
        public Telemetry Telemetry { get; private set; }
        public SessionData SessionData { get; private set; }

        public int CurrentSession { get; private set; }
        public bool IsReplay { get; set; }
        public SessionInfo SessionInfo { get; private set; }
        public List<Driver> Drivers { get; private set; }
        public Driver CurrentDriver { get; private set; }
        public Driver Leader { get; private set; }
        public TimeDelta TimeDelta { get; private set; }

        public void Start()
        {
            Reset();
            iRacing.StartListening();
        }

        public void Stop()
        {
            iRacing.StopListening();
            Reset();
        }


        private bool _mustReloadDrivers;
        private bool _mustUpdateSessionData;
        private bool _isUpdatingDrivers;

        private void Reset()
        {
            _mustUpdateSessionData = true;
            _mustReloadDrivers = true;
            _isUpdatingDrivers = false;
            

            Drivers.Clear();
            CurrentDriver = null;
            Leader = null;
            Telemetry = null;
            SessionData = null;
        }

        private void UpdateDriverList(SessionData sessionData)
        {
            _isUpdatingDrivers = true;
            GetDrivers(sessionData);
            _isUpdatingDrivers = false;

        }

        private void GetDrivers(SessionData sessionData)
        {
            if (_mustReloadDrivers)
            {
                Drivers.Clear();
                _mustReloadDrivers = false;
            }

            foreach (var driverInfo in sessionData.DriverInfo.Drivers)
            {
                var driver = Drivers.SingleOrDefault(driver => driver.Id == driverInfo.CarIdx);
                if (driver == null)
                {
                    driver = Driver.FromSessionData(sessionData, driverInfo.CarIdx);

                    if(driver == null) break;

                    Drivers.Add(driver);
                }
                else
                {
                    var newDriver = Driver.FromSessionData(sessionData, driverInfo.CarIdx);

                    Drivers.Remove(driver);
                    Drivers.Add(newDriver);
                }
            }
        }

        //private void GetResults(SessionData sessionData)
        //{
        //    // If currently updating list, or no session yet, then no need to update result info 
        //    if (_isUpdatingDrivers) return;
        //    if (CurrentSession == null) return;

        //    GetQualyResults(sessionData);
        //    GetRaceResults(sessionData);
        //}
        
        //private void GetQualyResults(SessionData sessionData)
        //{
        //    var qualify = sessionData.SessionInfo.Sessions.Qualifying();

        //    foreach (var pos in qualify.ResultsPositions)
        //    {
        //        var driver = Drivers.SingleOrDefault(x => x.Id == pos.CarIdx);
        //        driver?.UpdateQualyResults(pos);
        //    }
        //}

        //private void GetRaceResults(SessionData sessionData)
        //{
        //    var race = sessionData.SessionInfo.Sessions[CurrentSession.GetValueOrDefault()];

        //    foreach (var position in race.ResultsPositions)
        //    {
        //        var driver = Drivers.SingleOrDefault(x => x.Id == position.CarIdx);

        //        driver?.UpdateRaceResults(CurrentSession.GetValueOrDefault(), position);

        //    }
        //}

        private void UpdateDriverTelemetry(Telemetry telemetry)
        {
            if (_isUpdatingDrivers) return;
            
            foreach (var driver in Drivers)
            {
                driver.Update(telemetry);
            }
        }

        
        private void CheckSessionFlagUpdates(SessionFlags prevFlags, SessionFlags curFlags)
        {
            var isGreen = prevFlags != SessionFlags.green && curFlags == SessionFlags.green ||
                          prevFlags != SessionFlags.startGo && curFlags == SessionFlags.startGo;

            var isYellow = prevFlags != SessionFlags.yellow && curFlags == SessionFlags.yellow;

            if (isGreen)
            {
                // Notify Green Event

                RaceEvent?.Invoke(this, new RaceEvent());
            }

            if (isYellow)
            {
                // Notify Yellow Event
                RaceEvent?.Invoke(this, new RaceEvent());
            }
        }


        private void IRacingOnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        private void IRacingOnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        private void IRacingOnNewData(Telemetry telemetry)
        {
            Telemetry = telemetry;
            IsReplay = telemetry.IsReplayPlaying;
            
            if (CurrentSession != Telemetry.SessionNum)
            {
                _mustUpdateSessionData = true;
                _mustReloadDrivers = true;
            }

            CurrentSession = Telemetry.SessionNum;

            var sessionWasFinished = SessionInfo.IsFinished;
            var prevFlags = SessionInfo.Flags;

            SessionInfo.UpdateState(Telemetry.SessionState);

            UpdateDriverTelemetry(Telemetry);

            SessionInfo.Update(Telemetry);

            CheckSessionFlagUpdates(prevFlags, SessionInfo.Flags);

            CurrentDriver = Drivers.FirstOrDefault(x => x.Id == telemetry.CamCarIdx);
            
            TelemetryUpdated?.Invoke(this, telemetry);
        }

       

        private void IRacingOnNewSessionData(SessionData sessionData)
        {
            SessionData = sessionData;

            if (_mustUpdateSessionData)
            {
                SessionInfo.Update(sessionData);
                TimeDelta = new TimeDelta(SessionInfo.Track.Length, 20, 64);

                _mustUpdateSessionData = false;

                StaticInfoChanged?.Invoke(this, EventArgs.Empty);
            }

            UpdateDriverList(sessionData);

            SessionInfoUpdated?.Invoke(this, sessionData);
        }

    }
}
