using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class StateHeaderBase : IRacingComponentBase
    {
        public bool IsYellow { get; set; }
        public bool IsFullYellow { get; set; }
        public bool IsWhite { get; set; }
        public bool IsCheckered { get; set; }

        public bool IsLimitedSessionLaps { get; set; }
        public bool IsLimitedTime { get; set; }
        public string SessionType { get; set; }
        public int Lap { get; set; }
        public string Laps { get; set; }
        public string TimeRemain { get; set; }
        public string Time { get; set; }
        public bool IsRace { get; set; }

        public StateHeaderBase() : base("state-header")
        {
            ClassMapper
                .If("show-checkered-flag", () => IsCheckered)
                .If("show-white-flag", () => IsWhite)
                .If("show-yellow-flag", () => IsYellow)
                .If("show-full-course-yellow-flag", () => IsFullYellow);

        }

        public override void Update(object sender, DataSample data)
        {
            var session = data.SessionData.SessionInfo.Sessions[data.Telemetry.SessionNum];

            IsWhite = data.Telemetry.IsFinalLap && !(data.Telemetry.SessionState == SessionState.CoolDown || data.Telemetry.SessionState == SessionState.Checkered);
            IsCheckered = (data.Telemetry.SessionState == SessionState.CoolDown || data.Telemetry.SessionState == SessionState.Checkered);
            IsFullYellow = data.Telemetry.SessionFlags == SessionFlags.yellowWaving;
            IsYellow = data.Telemetry.SessionFlags == SessionFlags.yellow;

            TimeRemain = data.Telemetry.SessionTime.Seconds().ToTimeString();
            Time = (data.Telemetry.SessionTime + data.Telemetry.SessionTimeRemain).Seconds().ToTimeString();
            IsLimitedTime = session.IsLimitedTime;
            IsLimitedSessionLaps = session.IsLimitedSessionLaps;
            IsRace = session.IsRace;
            Lap = data.Telemetry.Lap;
            Laps = session.SessionLaps;

            SessionType = session.SessionType;

            StateHasChanged();
        }

        public override void UpdateSession(object sender, DataSample data)
        {
            
        }
    }
}
