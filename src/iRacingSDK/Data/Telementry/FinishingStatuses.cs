using System.Collections.Generic;
using iRacingSDK.Messaging;

namespace iRacingSDK.Data
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
