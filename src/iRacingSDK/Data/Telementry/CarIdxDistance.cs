using System.Collections.Generic;
using System.Linq;

namespace iRacingSDK.Data
{
	public partial class Telemetry : Dictionary<string, object>
	{
		float[] carIdxDistance;
		public float[] CarIdxDistance
		{
			get
			{
				if (carIdxDistance == null)
					carIdxDistance = Enumerable.Range(0, 64)
						.Select<int, float>(CarIdx => this.CarIdxLap[CarIdx] + this.CarIdxLapDistPct[CarIdx])
						.ToArray();

				return carIdxDistance;
			}
			internal set
			{
				carIdxDistance = value;
			}
		}

	}
}
