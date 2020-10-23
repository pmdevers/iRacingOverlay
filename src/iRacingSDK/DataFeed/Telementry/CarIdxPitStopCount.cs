using System;
using System.Collections.Generic;
using System.Text;

namespace iRacingSDK
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
