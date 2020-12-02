using System.Collections.Generic;

namespace iRacingSDK.Data
{
	public partial class Telemetry : Dictionary<string, object>
	{
		int[] carIdxPitStopCount;
		public int[] CarIdxPitStopCount
		{
			get
			{
				return carIdxPitStopCount;
			}
			set
			{
				carIdxPitStopCount = value;
			}
		}
	}
}
