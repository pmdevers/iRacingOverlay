using System.Diagnostics;

namespace iRacingSDK.Logging
{
	public static class TraceError
	{
		const string Category = "ERROR";

		public static void WriteLine(string value, params object[] args)
		{
			Trace.WriteLine(string.Format(value, args), Category);
		}

		public static void Write(string value, params object[] args)
		{
			Trace.Write(string.Format(value, args), Category);
		}

		public static void WriteLineIf(bool condition, string value, params object[] args)
		{
			Trace.WriteLineIf(condition, string.Format(value, args), Category);
		}

		public static void WriteIf(bool condition, string value, params object[] args)
		{
			Trace.WriteIf(condition, string.Format(value, args), Category);
		}
	}
}
