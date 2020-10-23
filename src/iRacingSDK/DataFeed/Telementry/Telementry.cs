using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iRacingSDK
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
					throw new Exception("Attempt to load car details for negative car index {0}".F(carIdx));

				if (carIdx >= cars.Length)
					throw new Exception("Attempt to load car details for unknown carIndex.  carIdx: {0}, maxNumber: {1}".F(carIdx, cars.Length - 1));

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

		public Car CamCar { get { return Cars[CamCarIdx]; } }

		CarArray cars;
		public CarArray Cars
		{
			get
			{
				if (cars != null)
					return cars;

				return cars = new CarArray(this);
			}
		}

		public CarDetails[] CarDetails { get { return Cars.Select(c => c.Details).ToArray(); } }

		public IEnumerable<Car> RaceCars
		{
			get
			{
				return Cars.Where(c => !c.Details.IsPaceCar);
			}
		}

		public bool UnderPaceCar
		{
			get
			{
				return this.CarIdxTrackSurface[0] == TrackLocation.OnTrack;
			}
		}

		public Dictionary<string, string> Descriptions { get; internal set; }

		public override string ToString()
		{
			var result = new StringBuilder();

			foreach (var kv in this)
			{
				var key = kv.Key;
				var description = (Descriptions != null && Descriptions.ContainsKey(key)) ? Descriptions[key] : "";
				var value = ConvertToSpecificType(key, kv.Value);

				var type = value.GetType().ToString();

				result.Append("TeleKey: | {0,-30} | {1,-30} | {2,30} | {3}\n".F(key, type, value, description));
			}

			return result.ToString();
		}

		object ConvertToSpecificType(string key, object value)
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
