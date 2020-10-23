using System.Collections.Generic;

namespace iRacingSDK
{
	public partial class Telemetry : Dictionary<string, object>
	{
		public bool[] HasSeenCheckeredFlag;
		public bool IsFinalLap;
		public bool LeaderHasFinished;
		public bool[] HasRetired;

		public bool HasData(int carIdx)
		{
			return CarIdxTrackSurface[carIdx] != TrackLocation.NotInWorld;
		}
	}
}
