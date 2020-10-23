namespace iRacingSDK
{
	public static class StringExtensions
	{
		public static string F(this string self, params object[] args)
		{
			return string.Format(self, args);
		}
	}
}
