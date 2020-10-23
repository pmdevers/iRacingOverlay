using System;
using System.Collections.Generic;
using System.Text;

namespace iRacingSDK
{
	public partial class SessionData
	{
		public partial class _SessionInfo
		{
			public partial class _Sessions
			{
				public int _SessionLaps
				{
					get
					{
						int result = 0;
						int.TryParse(SessionLaps, out result);
						return result;
					}
				}

				public double _SessionTime
				{
					get
					{
						double result = 0;
						double.TryParse(SessionTime.Replace(" sec", ""), out result);
						return result;
					}
				}

				public bool IsLimitedSessionLaps
				{
					get
					{
						return SessionLaps.ToLower() != "unlimited";
					}
				}

				public bool IsLimitedTime
				{
					get
					{
						return SessionTime.ToLower() != "unlimited";
					}
				}
			}
		}
	}
}
