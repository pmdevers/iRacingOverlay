using System;
using System.Collections.Generic;

namespace iRacingSDK.Data
{
	[Serializable]
	public class DataSample
	{
		private Telemetry _telemetry;
		private SessionData _sessionData;

		public static readonly DataSample YetToConnected = new DataSample { IsConnected = false };
		public DataSample LastSample { get; set; }

		public bool IsConnected { get; internal set; }
		public SessionData SessionData
		{
			get
			{
				if (!IsConnected)
					throw new Exception("Attempt to read session data before connection to iRacing");

				if (_sessionData == null)
					throw new Exception("SessionData is null");

				return _sessionData;
			}
			set => _sessionData = value;
        }

		public Telemetry Telemetry
		{
			get
			{
				if (!IsConnected)
					throw new Exception("Attempt to read telemetry data before connection to iRacing");

				if (_telemetry == null)
					throw new Exception("OnTelemetry is null");

				return _telemetry;
			}
			set => _telemetry = value;
        }

		public Dictionary<string, string> Descriptions { get; internal set; }
	}
}
