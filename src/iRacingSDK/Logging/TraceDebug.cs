using System.Diagnostics;

namespace iRacingSDK.Logging
{
	public static class TraceDebug
	{
		const string Category = "DEBUG";

		public static void WriteLine(string value, params object[] args)
		{
			Trace.WriteLine(value.F(args), Category);
		}

		public static void Write(string value, params object[] args)
		{
			Trace.Write(value.F(args), Category);
		}

		public static void WriteLineIf(bool condition, string value, params object[] args)
		{
			Trace.WriteLineIf(condition, value.F(args), Category);
		}

		public static void WriteIf(bool condition, string value, params object[] args)
		{
			Trace.WriteIf(condition, value.F(args), Category);
		}
	}
}
