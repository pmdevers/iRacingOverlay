using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRacingSDK.Logging;

namespace iRacingSDK
{
	public class CarDetails
	{
		readonly int carIdx;
		readonly Telemetry telemetry;
		readonly SessionData._DriverInfo._Drivers driver;

		public CarDetails(Telemetry telemetry, int carIdx)
		{
			this.telemetry = telemetry;
			this.carIdx = carIdx;
			this.driver = telemetry.SessionData.DriverInfo.CompetingDrivers[carIdx];
		}

		public int Index { get { return carIdx; } }
		public int CarIdx { get { return carIdx; } }
		public SessionData._DriverInfo._Drivers Driver { get { return driver; } }
		public string CarNumberDisplay { get { return driver == null ? "" : driver.CarNumber; } }
		public short CarNumberRaw { get { return driver == null ? (short)-1 : (short)driver.CarNumberRaw; } }
		public string UserName { get { return driver == null ? "Unknown" : driver.UserName; } }
		public bool IsPaceCar { get { return carIdx == 0; } }


		public Car Car(DataSample data)
		{
			return data.Telemetry.Cars[this.carIdx];
		}
	}

	public class Car
	{
		readonly int carIdx;
		readonly Telemetry telemetry;
		readonly SessionData._DriverInfo._Drivers driver;
		public readonly CarDetails Details;

		public Car(Telemetry telemetry, int carIdx)
		{
			this.telemetry = telemetry;
			this.carIdx = carIdx;
			this.driver = telemetry.SessionData.DriverInfo.CompetingDrivers[carIdx];
			this.Details = new CarDetails(telemetry, carIdx);
		}

		public int Index { get { return carIdx; } }
		public int CarIdx { get { return carIdx; } }

		public int Lap { get { return telemetry.CarIdxLap[carIdx]; } }
		public float DistancePercentage { get { return telemetry.CarIdxLapDistPct[carIdx]; } }
		public float TotalDistance { get { return this.Lap + this.DistancePercentage; } }
		public LapSector LapSector { get { return telemetry.CarSectorIdx[carIdx]; } }
		public int Position { get { return telemetry.Positions[carIdx]; } }
		public int OfficialPostion { get { return telemetry.CarIdxPosition[carIdx]; } }
		public bool HasSeenCheckeredFlag { get { return telemetry.HasSeenCheckeredFlag[carIdx]; } }
		public bool HasData { get { return telemetry.HasData(carIdx); } }
		public bool HasRetired { get { return telemetry.HasRetired[carIdx]; } }
		public TrackLocation TrackSurface { get { return telemetry.CarIdxTrackSurface[carIdx]; } }
		public int PitStopCount { get { return telemetry.CarIdxPitStopCount[carIdx]; } }
		public bool IsInPits { get { return TrackSurface == TrackLocation.InPitStall; } }

		public SessionData._SessionInfo._Sessions._ResultsPositions ResultPosition
		{
			get
			{
				if (telemetry.Session.ResultsPositions == null)
					return null;

				return telemetry.Session.ResultsPositions.FirstOrDefault(rp => rp.CarIdx == carIdx);
			}
		}

		public TimeSpan LastTimeSpan
		{
			get { return LastTime.Seconds(); }
		}

		public double LastTime
		{
			get
			{
				var rp = ResultPosition;
				if (rp == null)
					return 0f;

				if (rp.LapsComplete != (Lap - 1))
				{
					TraceInfo.WriteLine("Attempt to get LastTime from session data, with mismatch Lap counters.  Telemerty Lap: {0}.  Session LapComplete: {1}", Lap - 1, rp.LapsComplete);
					return 0f;
				}

				return rp.LastTime;
			}
		}
	}
}
