using System;
using System.Collections.Generic;

namespace iRacingSDK.Data
{
	public partial class Telemetry : Dictionary<string, object>
	{
		TimeSpan? sessionTimeSpan;
		public TimeSpan SessionTimeSpan
		{
			get
			{
				if (sessionTimeSpan == null)
					sessionTimeSpan = TimeSpan.FromSeconds(SessionTime);

				return sessionTimeSpan.Value;
			}
		}
	}
}
