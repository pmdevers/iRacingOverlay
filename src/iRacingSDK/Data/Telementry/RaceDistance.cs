using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace iRacingSDK.Data
{
	public partial class Telemetry : Dictionary<string, object>
	{
		float? raceDistance;

		public float RaceDistance
		{
			get
			{
				if (raceDistance != null)
					return raceDistance.Value;

				raceDistance = Enumerable.Select(this.CarIdxLap, (lap, idx) => new { Lap = lap, Distance = lap + this.CarIdxLapDistPct[idx] })
					.Max(l => l.Distance);

				if (raceDistance.Value < this.RaceLaps)
				{
					Trace.WriteLine("WARNING! No cars on current RaceLaps", "DEBUG");
					return this.RaceLaps;
				}

				return raceDistance.Value;
			}
		}
	}
}
