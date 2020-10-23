using System;

namespace iRacingSDK
{
	public static class TimeSpanExtensions
	{
		public static TimeSpan Seconds(this int seconds)
		{
			return TimeSpan.FromSeconds(seconds);
		}

		public static TimeSpan Second(this int seconds)
		{
			return TimeSpan.FromSeconds(seconds);
		}

		public static TimeSpan Seconds(this double seconds)
		{
			return TimeSpan.FromSeconds(seconds);
		}

		public static TimeSpan Minutes(this int minutes)
		{
			return TimeSpan.FromMinutes(minutes);
		}

		public static TimeSpan Minutes(this double minutes)
		{
			return TimeSpan.FromMinutes(minutes);
		}
	}
}
