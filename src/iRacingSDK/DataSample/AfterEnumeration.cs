using System;
using System.Collections.Generic;
using System.Text;
using iRacingSDK.Logging;

namespace iRacingSDK
{
	public class AfterEnumeration
	{
		readonly IEnumerable<DataSample> _samples;
		readonly TimeSpan _period;

		public AfterEnumeration(IEnumerable<DataSample> samples, TimeSpan period)
		{
			_samples = samples;
			_period = period;
		}

		/// <summary>
		/// Once supplied function returns true, iteration stops after the specified period
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public IEnumerable<DataSample> After(Func<DataSample, bool> condition)
		{
			var conditionMet = false;
			var conditionMetAt = new TimeSpan();

			foreach (var data in _samples)
			{
				if (!conditionMet && condition(data))
				{
					conditionMet = true;
					conditionMetAt = data.Telemetry.SessionTimeSpan;
				}

				if (conditionMet && conditionMetAt + _period < data.Telemetry.SessionTimeSpan)
					break;

				yield return data;
			}
		}

		/// <summary>
		/// If the supplied function returns true for the period, then iteration stops
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public IEnumerable<DataSample> Of(Func<DataSample, bool> condition)
		{
			var conditionMet = false;
			var conditionMetAt = new TimeSpan();

			foreach (var data in _samples)
			{
				if (condition(data))
				{
					if (!conditionMet)
					{
						TraceDebug.WriteLine("{0}: Condition met".F(data.Telemetry.SessionTimeSpan));
						conditionMet = true;
						conditionMetAt = data.Telemetry.SessionTimeSpan;
					}
				}
				else
				{
					if (conditionMet)
						TraceDebug.WriteLine("{0}: Condition unmet".F(data.Telemetry.SessionTimeSpan));
					conditionMet = false;
				}


				if (conditionMet && conditionMetAt + _period < data.Telemetry.SessionTimeSpan)
					break;

				yield return data;
			}
		}

		public IEnumerable<DataSample> AfterReplayPaused()
		{
			var timeoutAt = DateTime.Now + _period;
			var lastFrameNumber = -1;

			foreach (var data in _samples)
			{
				if (lastFrameNumber == data.Telemetry.ReplayFrameNum)
				{
					if (timeoutAt < DateTime.Now)
					{
						TraceInfo.WriteLine("{0} Replay paused for {1}.  Assuming end of replay", data.Telemetry.SessionTimeSpan, _period);
						break;
					}
				}
				else
				{
					timeoutAt = DateTime.Now + _period;
					lastFrameNumber = data.Telemetry.ReplayFrameNum;
				}

				yield return data;
			}
		}
	}
}
