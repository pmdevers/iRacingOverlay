using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using iRacingSDK.Data;
using iRacingSDK.Messaging;

namespace iRacingSDK
{
	public static class iRacing
	{
        private static readonly iRacingConnection Instance = new iRacingConnection();
        private static readonly iRacingEvents EventInstance = new iRacingEvents();

		public static Replay Replay => Instance.Replay;
        public static PitCommand PitCommand => Instance.PitCommand;
        public static Camera Camera => Instance.Camera;
        public static Chat Chat => Instance.Chat;

        public static bool IsConnected => Instance.IsConnected;
        public static bool IsListening => EventInstance.IsListening;

        public static SessionData Session { get; private set; }
        public static SessionData._SessionInfo._Sessions Current => Session?.SessionInfo.Sessions[CurrentSessionNumber];
        public static Telemetry Telemetry { get; private set; }
        public static int CurrentSessionNumber { get; private set; }

        public static bool LeaderHasFinished => Telemetry.RaceLaps > Current.ResultsLapsComplete;
        public static bool IsFinalLap => Telemetry.RaceLaps >= Current.ResultsLapsComplete;

        public static DriversCollection Drivers { get; } = new DriversCollection();

        static iRacing()
        {
            EventInstance.OnTelemetry += TelemetryUpdated;
            EventInstance.OnSessionChanged += SessionChanged;
        }

        private static void SessionChanged(SessionData session)
        {
            Session = session;

            Drivers.Update(session);
        }

        private static void TelemetryUpdated(Telemetry telemetry)
        {
            Telemetry = telemetry;

            if (CurrentSessionNumber != telemetry.SessionNum)
            {
                Drivers.Clear();
                Drivers.Update(telemetry.SessionData);
            }

            CurrentSessionNumber = telemetry.SessionNum;
            
            Drivers.Update(telemetry);
        }

        public static IEnumerable<DataSample> GetDataFeed()
		{
			return Instance.GetDataFeed();
		}

		public static void StartListening()
		{
			EventInstance.StartListening();
		}

		public static void StopListening()
		{
			EventInstance.StopListening();
		}

		public static event Action<Telemetry> OnTelemetry
		{
			add => EventInstance.OnTelemetry += value;
            remove => EventInstance.OnTelemetry -= value;
        }

		public static event Action<SessionData> OnSessionChanged
        {
            add => EventInstance.OnSessionChanged += value;
            remove => EventInstance.OnSessionChanged -= value;
        }



	}
}
