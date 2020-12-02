using System.Collections.Generic;
using System.Linq;

namespace iRacingSDK.Data
{
	public partial class Telemetry : Dictionary<string, object>
	{
		LapSector? _raceLapSector;
		public LapSector RaceLapSector
		{
			get
			{
				if (_raceLapSector != null)
					return _raceLapSector.Value;

				var firstSector = Enumerable.Select(this.CarIdxLap, (lap, idx) => new { Lap = lap, Idx = idx, Pct = this.CarIdxLapDistPct[idx] })
					.Where(l => l.Lap == this.RaceLaps)
					.OrderByDescending(l => l.Pct)
					.FirstOrDefault();

				if (firstSector == null)
					return (_raceLapSector = new LapSector(this.RaceLaps, 2)).Value;

				return (_raceLapSector = new LapSector(this.RaceLaps, ToSectorFromPercentage(firstSector.Pct))).Value;
			}
		}
	}
}
