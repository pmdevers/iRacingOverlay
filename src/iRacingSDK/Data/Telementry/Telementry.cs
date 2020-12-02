using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRacingSDK.Messaging;

namespace iRacingSDK.Data
{
	public class CarArray : IEnumerable<Car>
	{
		Car[] cars;

		public CarArray(Telemetry telemetry)
		{
			var drivers = telemetry.SessionData.DriverInfo.CompetingDrivers;

			cars = new Car[drivers.Length];

			for (var i = 0; i < drivers.Length; i++)
				cars[i] = new Car(telemetry, i);
		}

		public Car this[long carIdx]
		{
			get
			{
				if (carIdx < 0)
					throw new Exception($"Attempt to load car details for negative car index {carIdx}");

				if (carIdx >= cars.Length)
					throw new Exception($"Attempt to load car details for unknown carIndex.  carIdx: {carIdx}, maxNumber: {cars.Length - 1}");

				return cars[carIdx];
			}
		}

		public IEnumerator<Car> GetEnumerator()
		{
			return (cars as IEnumerable<Car>).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return cars.GetEnumerator();
		}
	}

	public partial class Telemetry : Dictionary<string, object>
	{
		public SessionData._SessionInfo._Sessions Session
		{
			get
			{
				if (SessionNum < 0 || SessionNum >= SessionData.SessionInfo.Sessions.Length)
					return null;

				return SessionData.SessionInfo.Sessions[SessionNum];
			}
		}

		public Car CamCar => Cars[CamCarIdx];

        CarArray _cars;
		public CarArray Cars
		{
			get
			{
				if (_cars != null)
					return _cars;

				return _cars = new CarArray(this);
			}
		}

		public CarDetails[] CarDetails => Cars.Select(c => c.Details).ToArray();

        public IEnumerable<Car> RaceCars => Cars.Where(c => !c.Details.IsPaceCar);

        public bool UnderPaceCar => this.CarIdxTrackSurface[0] == TrackLocation.OnTrack;

        public override string ToString()
		{
			var result = new StringBuilder();

			foreach (var kv in this)
			{
				var key = kv.Key;
				var description = (Descriptions != null && Descriptions.ContainsKey(key)) ? Descriptions[key] : "";
				var value = ConvertToSpecificType(key, kv.Value);

				var type = value.GetType().ToString();

				result.Append($"TeleKey: | {key,-30} | {type,-30} | {value,30} | {description}\n");
			}

			return result.ToString();
		}

        private object ConvertToSpecificType(string key, object value)
		{
			switch (key)
			{
				case "SessionState":
					return (SessionState)(int)value;

				case "SessionFlags":
					return (SessionFlags)(int)value;

				case "EngineWarnings":
					return (EngineWarnings)(int)value;

				case "CarIdxTrackSurface":
					return ((int[])value).Select(v => (TrackLocation)v).ToArray();

				case "DisplayUnits":
					return (DisplayUnits)(int)value;

				case "WeatherType":
					return (WeatherType)(int)value;

				case "Skies":
					return (Skies)(int)value;
			}

			return value;
		}
	}
}
