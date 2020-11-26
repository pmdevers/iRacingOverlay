using System;
using System.Text;

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

        public static string ToTimeString(this TimeSpan ts)
        {
            if (ts == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            var hours = (int) ts.TotalHours;

			if (hours > 0)
            {
                sb.Append(hours.ToString().PadLeft(2, '0'));
                sb.Append(":");
            }

            sb.Append(ts.Minutes.ToString().PadLeft(2, '0'));
            sb.Append(":");
            sb.Append(ts.Seconds.ToString().PadLeft(2, '0'));

            return sb.ToString();
		}

        public static string ToLapTimeString(this TimeSpan ts)
        {
            return ts.ToString(@"m\:ss\.fff");
        }
	}
}
