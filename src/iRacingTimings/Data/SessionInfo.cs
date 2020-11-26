using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;

namespace iRacingTimings.Data
{
    public class SessionInfo
    {
        public Track Track { get; private set; }
        public string SessionType { get; private set; }
        public Time SessionTime { get; private set; }
        public Time TimeRemaining { get; private set; }
        public Time RaceTime { get; set; }
        public string RaceLaps { get; set; }
        public bool IsRace { get; private set; }
        public bool IsCheckered { get; private set; }
        public bool IsFinished { get; private set; }
        public SessionFlags Flags { get; private set; }
        public bool IsLimitedSessionLaps { get; private set; }
        public bool IsLimitedTime { get; private set; }

        public string EventType { get; private set; }
        

        public void Update(SessionData sessionData)
        {
            Track = Track.FromSessionData(sessionData);

            var session = sessionData.SessionInfo.Sessions[Simulator.Instance.CurrentSession];

            SessionType = session.SessionType;

            IsLimitedSessionLaps = session.IsLimitedSessionLaps;
            IsLimitedTime = session.IsLimitedTime;

            RaceTime = session._SessionTime;
            RaceLaps = session.SessionLaps;
        }

        

        public void Update(Telemetry telemetry)
        {
            SessionTime = telemetry.SessionTime;
            TimeRemaining = telemetry.SessionTimeRemain;
        }

        public void UpdateState(SessionState state)
        {
            IsFinished = state == SessionState.CoolDown;
            IsCheckered = (state == SessionState.Checkered || state == SessionState.CoolDown);
        }

        
    }
}
