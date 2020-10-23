using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSDK.Logging;

namespace iRacingSDK
{
	public static class DataSampleExtensions
	{
		public static IEnumerable<DataSample> AtSpeed(this IEnumerable<DataSample> samples, int replaySpeed,
			Func<DataSample, bool> fn)
		{
			var speedBeenSet = false;

			foreach (var data in samples)
			{
				if (fn(data) && !speedBeenSet && data.Telemetry.ReplayPlaySpeed != replaySpeed)
				{
					iRacing.Replay.SetSpeed(replaySpeed);
					speedBeenSet = true;
				}

				if (speedBeenSet)
					if (data.Telemetry.ReplayPlaySpeed == replaySpeed)
						speedBeenSet = false;

				yield return data;
			}
		}

		public static IEnumerable<DataSample> AtSpeed(this IEnumerable<DataSample> samples, int replaySpeed)
		{
			return AtSpeed(samples, replaySpeed, (data) => true);
		}

		public static void EmitTo(this IEnumerable<DataSample> samples, Func<DataSample, bool> emitter)
		{
			foreach (var d in samples)
				if (!emitter(d))
					return;
		}

		/// <summary>
		/// Logs an error is frame numbers goes down - indicating the game is replaying in reverse.
		/// Sometimes stream may glitch and the FrameNum decements
		/// </summary>
		public static IEnumerable<DataSample> ForwardOnly(this IEnumerable<DataSample> samples)
		{
			foreach (var data in samples)
			{
				if (data.LastSample != null && data.LastSample.Telemetry.ReplayFrameNum > data.Telemetry.ReplayFrameNum)
					TraceInfo.WriteLine(
						"WARNING! Replay data reversed.  Current enumeration only support iRacing in forward mode. Received sample {0} after sample {1}",
						data.Telemetry.ReplayFrameNum, data.LastSample.Telemetry.ReplayFrameNum);
				else
					yield return data;
			}
		}

		/// <summary>
		/// Similiar to GetDataFeed, except DataSample can be buffered upto maxBufferLength to asist in reducing loss of data packets
		/// Therefore, DataSamples yield from this enumeration may have a higher latency of values.
		/// </summary>
		/// <param name="iRacingConnection"></param>
		/// <param name="maxBufferLength"></param>
		/// <returns></returns>
		public static IEnumerable<DataSample> GetBufferedDataFeed(this iRacingConnection iRacingConnection,
			int maxBufferLength = 10)
		{
			return _GetBufferedDataFeed(iRacingConnection, maxBufferLength).WithLastSample();
		}

		static IEnumerable<DataSample> _GetBufferedDataFeed(iRacingConnection iRacingConnection, int maxBufferLength)
		{
			var que = new ConcurrentQueue<DataSample>();
			bool cancelRequest = false;

			var t = new Task(() => EnqueueSamples(que, iRacingConnection, maxBufferLength, ref cancelRequest));
			t.Start();

			try
			{
				DataSample data;

				while (true)
				{
					if (que.TryDequeue(out data))
						yield return data;
				}
			}
			finally
			{
				cancelRequest = true;
				t.Wait(200);
				t.Dispose();
			}
		}

		static void EnqueueSamples(ConcurrentQueue<DataSample> que, iRacingConnection samples, int maxBufferLength,
			ref bool cancelRequest)
		{
			foreach (var data in samples.GetRawDataFeed())
			{
				if (cancelRequest)
					return;

				if (que.Count < maxBufferLength)
					que.Enqueue(data);
				else
					Debug.WriteLine(string.Format("Dropped DataSample {0}.", data.Telemetry.TickCount));
			}
		}


		[Obsolete("See 'RaceIncidents2'")]
		public static IEnumerable<DataSample> RaceIncidents(this IEnumerable<DataSample> samples,
			int maxTotalIncidents = int.MaxValue)
		{
			return RaceIncidents2(samples, 100, maxTotalIncidents);
		}

		/// <summary>
		/// Move to start of Race.
		/// Then advances the game through each incident until the end of race, or until NextIncident fails to advance
		/// Then does the same in reverse order (from race end to race start) - to ensure we get all incidents.
		/// </summary>
		/// <param name="samples"></param>
		/// <param name="maxTotalIncidents"></param>
		/// <returns>Return a DataSample of each frame that an identified incident occured on.</returns>
		public static IEnumerable<DataSample> RaceIncidents2(this IEnumerable<DataSample> samples, int sampleScanSettle,
			int maxTotalIncidents = int.MaxValue)
		{
			var sessionNumber = GetSessionNumber(samples);

			var incidentsOnForward = GetIncidentsForward(samples, sampleScanSettle, maxTotalIncidents);

			var incidents = incidentsOnForward
				.OrderBy(d => d.Telemetry.ReplayFrameNum)
				.ToList();

			foreach (var incident in incidents)
				Trace.WriteLine(
					string.Format("Found new incident at frame {0} for {1}", incident.Telemetry.SessionTimeSpan,
						incident.Telemetry.CamCar.Details.UserName), "DEBUG");

			return incidents;
		}

		static int GetSessionNumber(IEnumerable<DataSample> samples)
		{
			var data = samples.First();
			return data.Telemetry.SessionNum;
		}

		static List<DataSample> GetIncidentsForward(IEnumerable<DataSample> samples, int sampleScanSettle, int maxTotalIncidents)
		{
			TraceDebug.WriteLine("Scanning for incidents forwards from start");

			return IncidentsSupport.FindIncidents(
				samples.TakeWhile(data => data.Telemetry.SessionState != SessionState.CoolDown),
				sampleScanSettle,
				maxTotalIncidents);

		}

		public static IEnumerable<DataSample> RaceOnly(this IEnumerable<DataSample> samples)
		{
			iRacing.Replay.MoveToStartOfRace();

			foreach (var data in samples)
			{
				if (data.Telemetry.SessionState == SessionState.Checkered)
					break;

				if (data.Telemetry.SessionState != SessionState.Racing)
					continue;

				yield return data;
			}
		}

		public static AfterEnumeration TakeUntil(this IEnumerable<DataSample> samples, TimeSpan period)
		{
			return new AfterEnumeration(samples, period);
		}

		/// <summary>
		/// Assume replay is playing forward
		/// If Frame numbers do not advance after 100 identical framenumbers, the enumerator is stopped
		/// </summary>
		public static IEnumerable<DataSample> TakeUntilEndOfReplay(this IEnumerable<DataSample> samples)
		{
			const int maxRetryCount = 100;
			var retryCount = maxRetryCount;
			var lastFrameNumber = -1;

			foreach (var data in samples)
			{
				if (lastFrameNumber == data.Telemetry.ReplayFrameNum)
				{
					if (retryCount-- <= 0)
						break;
				}
				else
				{
					retryCount = maxRetryCount;
					lastFrameNumber = data.Telemetry.ReplayFrameNum;
				}

				yield return data;
			}
		}

		/// <summary>
		/// Filters out DataSamples that have a ReplayFrameNumber of 0.
		/// </summary>
		public static IEnumerable<DataSample> VerifyReplayFrames(this IEnumerable<DataSample> samples)
		{
			bool hasLoggedBadReplayFrameNum = false;
			bool hasLoggedBadSessionNum = false;

			foreach (var data in samples)
			{
				if (data.Telemetry.ReplayFrameNum == 0)
				{
					Trace.WriteLine("Received bad sample.  No ReplayFrameNumber.", "DEBUG");
					if (!hasLoggedBadReplayFrameNum)
					{
						Trace.WriteLine(data.Telemetry.ToString(), "DEBUG");
						hasLoggedBadReplayFrameNum = true;
					}
				}
				else if (data.Telemetry.Session == null)
				{
					Trace.WriteLine("Received bad sample.  Invalid SessionNum.", "DEBUG");
					if (!hasLoggedBadSessionNum)
					{
						Trace.WriteLine(data.Telemetry.ToString(), "DEBUG");
						hasLoggedBadSessionNum = true;
					}
				}
				else
					yield return data;
			}
		}

		/// <summary>
		/// filter the DataSamples, and correct for when a car blips off due to data loss - and reports laps/distances as -1
		/// Ensures the lap/distances measures are only progressing upwards
		/// Does not support streaming across sessions, where laps/distrance will naturally go down
		/// Also does not support is gaming is playing replay in reverse.
		/// </summary>
		/// <param name="samples"></param>
		/// <returns></returns>
		public static IEnumerable<DataSample> WithCorrectedDistances(this IEnumerable<DataSample> samples)
		{
			var maxDistance = new float[64];
			var lastAdjustment = new int[64];

			foreach (var data in samples.ForwardOnly())
			{
				for (int i = 0; i < data.SessionData.DriverInfo.CompetingDrivers.Length; i++)
					CorrectDistance(data.SessionData.DriverInfo.CompetingDrivers[i].UserName,
						ref data.Telemetry.CarIdxLap[i],
						ref data.Telemetry.CarIdxLapDistPct[i],
						ref maxDistance[i],
						ref lastAdjustment[i]);

				yield return data;
			}
		}

		static void CorrectDistance(string driverName, ref int lap, ref float distance, ref float maxDistance, ref int lastAdjustment)
		{
			var totalDistance = lap + distance;
			var roundedDistance = (int)(totalDistance * 1000.0);
			var roundedMaxDistance = (int)(maxDistance * 1000.0);

			if (roundedDistance > roundedMaxDistance && roundedDistance > 0)
				maxDistance = totalDistance;

			if (roundedDistance < roundedMaxDistance)
			{
				lastAdjustment = roundedDistance;
				lap = (int)maxDistance;
				distance = maxDistance - (int)maxDistance;
			}
		}

		/// <summary>
		/// Work around bug in iRacing data stream, where cars lap percentage is reported slightly behind 
		/// actual frame - so that as cars cross the line, their percentage still is in the 99% range
		/// a frame later there percentage drops to near 0%
		/// Fix is to watch for lap change - and zero percentage until less than 90%
		/// </summary>
		/// <param name="samples"></param>
		/// <returns></returns>
		public static IEnumerable<DataSample> WithCorrectedPercentages(this IEnumerable<DataSample> samples)
		{
			int[] lastLaps = null;

			foreach (var data in samples.ForwardOnly())
			{
				if (lastLaps == null)
					lastLaps = (int[])data.Telemetry.CarIdxLap.Clone();

				for (int i = 0; i < data.SessionData.DriverInfo.CompetingDrivers.Length; i++)
					if (data.Telemetry.HasData(i))
						FixPercentagesOnLapChange(
							ref lastLaps[i],
							ref data.Telemetry.CarIdxLapDistPct[i],
							data.Telemetry.CarIdxLap[i]);

				yield return data;
			}
		}

		static void FixPercentagesOnLapChange(ref int lastLap, ref float carIdxLapDistPct, int carIdxLap)
		{
			if (carIdxLap > lastLap && carIdxLapDistPct > 0.80f)
				carIdxLapDistPct = 0;
			else
				lastLap = carIdxLap;
		}

		/// <summary>
		/// Internal use in sdk only.
		/// Raise the connection and disconnection events as iRacing is started, stopped.
		/// </summary>
		internal static IEnumerable<DataSample> WithEvents(this IEnumerable<DataSample> samples, CrossThreadEvents connectionEvent, CrossThreadEvents disconnectionEvent, CrossThreadEvents<DataSample> newSessionData)
		{
			var isConnected = false;
			var isDisconnected = true;
			var lastSessionInfoUpdate = -1;

			foreach (var data in samples)
			{
				if (!isConnected && data.IsConnected)
				{
					isConnected = true;
					isDisconnected = false;
					connectionEvent.Invoke();
				}

				if (!isDisconnected && !data.IsConnected)
				{
					isConnected = false;
					isDisconnected = true;
					disconnectionEvent.Invoke();
				}

				if (data.IsConnected && data.SessionData.InfoUpdate != lastSessionInfoUpdate)
				{
					lastSessionInfoUpdate = data.SessionData.InfoUpdate;
					newSessionData.Invoke(data);
				}

				yield return data;
			}
		}

		public static IEnumerable<DataSample> WithFastestLaps(this IEnumerable<DataSample> samples)
		{
			FastLap lastFastLap = null;
			var lastDriverLaps = new int[64];
			var driverLapStartTime = new double[64];
			var fastestLapTime = double.MaxValue;

			foreach (var data in samples.ForwardOnly())
			{
				var carsAndLaps = data.Telemetry
					.CarIdxLap
					.Select((l, i) => new { CarIdx = i, Lap = l })
					.Skip(1)
					.Take(data.SessionData.DriverInfo.CompetingDrivers.Length - 1);

				foreach (var lap in carsAndLaps)
				{
					if (lap.Lap == -1)
						continue;

					if (lap.Lap == lastDriverLaps[lap.CarIdx] + 1)
					{
						var lapTime = data.Telemetry.SessionTime - driverLapStartTime[lap.CarIdx];

						driverLapStartTime[lap.CarIdx] = data.Telemetry.SessionTime;
						lastDriverLaps[lap.CarIdx] = lap.Lap;

						if (lap.Lap > 1 && lapTime < fastestLapTime)
						{
							fastestLapTime = lapTime;

							lastFastLap = new FastLap
							{
								Time = TimeSpan.FromSeconds(lapTime),
								Driver = data.SessionData.DriverInfo.CompetingDrivers[lap.CarIdx]
							};
						}
					}
				}

				data.Telemetry.FastestLap = lastFastLap;

				yield return data;
			}
		}

		/// <summary>
		/// Assignes the telemetry fields HasSeenCheckeredFlag, IsFinalLap, LeaderHasFinished, HasRetired
		/// </summary>
		/// <param name="samples"></param>
		/// <returns></returns>
		public static IEnumerable<DataSample> WithFinishingStatus(this IEnumerable<DataSample> samples)
		{
			var hasSeenCheckeredFlag = new bool[64];
			var lastTimeForData = new TimeSpan[64];

			foreach (var data in samples)
			{
				ApplyIsFinalLap(data);

				ApplyLeaderHasFinished(data);

				ApplyHasSeenCheckeredFlag(data, hasSeenCheckeredFlag);

				ApplyHasRetired(data, lastTimeForData);

				yield return data;
			}
		}

		static void ApplyIsFinalLap(DataSample data)
		{
			data.Telemetry.IsFinalLap = data.Telemetry.RaceLaps >= data.SessionData.SessionInfo.Sessions[data.Telemetry.SessionNum].ResultsLapsComplete;
		}

		static void ApplyLeaderHasFinished(DataSample data)
		{
			if (data.Telemetry.RaceLaps > data.SessionData.SessionInfo.Sessions[data.Telemetry.SessionNum].ResultsLapsComplete)
				data.Telemetry.LeaderHasFinished = true;
		}

		static void ApplyHasSeenCheckeredFlag(DataSample data, bool[] hasSeenCheckeredFlag)
		{
			if (data.LastSample != null && data.Telemetry.LeaderHasFinished)
				for (int i = 1; i < data.SessionData.DriverInfo.CompetingDrivers.Length; i++)
					if (data.LastSample.Telemetry.CarIdxLapDistPct[i] > 0.90 && data.Telemetry.CarIdxLapDistPct[i] < 0.10)
						hasSeenCheckeredFlag[i] = true;

			data.Telemetry.HasSeenCheckeredFlag = hasSeenCheckeredFlag;
		}

		static void ApplyHasRetired(DataSample data, TimeSpan[] lastTimeOfData)
		{
			data.Telemetry.HasRetired = new bool[64];

			if (!(new[] { SessionState.Racing, SessionState.Checkered, SessionState.CoolDown }).Contains(data.Telemetry.SessionState))
				return;

			for (int i = 1; i < data.SessionData.DriverInfo.CompetingDrivers.Length; i++)
			{
				if (data.Telemetry.HasSeenCheckeredFlag[i])
					continue;

				if (data.Telemetry.HasData(i))
				{
					lastTimeOfData[i] = data.Telemetry.SessionTimeSpan;
					continue;
				}

				if (lastTimeOfData[i] + 30.Seconds() < data.Telemetry.SessionTimeSpan)
					data.Telemetry.HasRetired[i] = true;
			}
		}

		/// <summary>
		/// Mixes in the LastSample field 
		/// Also disconnect the link list - so only the immediate sample has ref to last sample.
		/// </summary>
		public static IEnumerable<DataSample> WithLastSample(this IEnumerable<DataSample> samples)
		{
			DataSample lastDataSample = null;

			foreach (var data in samples)
			{
				data.LastSample = lastDataSample;
				if (lastDataSample != null)
					lastDataSample.LastSample = null;
				lastDataSample = data;

				yield return data;
			}
		}

		/// <summary>
		/// Set the CarIdxPitStopCount field for each enumerted datasample's telemetry
		/// </summary>
		public static IEnumerable<DataSample> WithPitStopCounts(this IEnumerable<DataSample> samples)
		{
			var lastTrackLocation = Enumerable.Repeat(TrackLocation.NotInWorld, 64).ToArray();
			var carIdxPitStopCount = new int[64];

			foreach (var data in samples.ForwardOnly())
			{
				CapturePitStopCounts(lastTrackLocation, carIdxPitStopCount, data);

				data.Telemetry.CarIdxPitStopCount = (int[])carIdxPitStopCount.Clone();

				yield return data;
			}
		}

		static void CapturePitStopCounts(TrackLocation[] lastTrackLocation, int[] carIdxPitStopCount, DataSample data)
		{
			if (data.LastSample == null)
				return;

			CaptureLastTrackLocations(lastTrackLocation, data);
			IncrementPitStopCounts(lastTrackLocation, carIdxPitStopCount, data);
		}

		static void CaptureLastTrackLocations(TrackLocation[] lastTrackLocation, DataSample data)
		{
			var last = data.LastSample.Telemetry.CarIdxTrackSurface;
			for (var i = 0; i < last.Length; i++)
				if (last[i] != TrackLocation.NotInWorld)
					lastTrackLocation[i] = last[i];
		}

		static void IncrementPitStopCounts(TrackLocation[] lastTrackLocation, int[] carIdxPitStopCount, DataSample data)
		{
			var current = data.Telemetry.CarIdxTrackSurface;
			for (var i = 0; i < current.Length; i++)
				if (lastTrackLocation[i] != TrackLocation.InPitStall && current[i] == TrackLocation.InPitStall)
					carIdxPitStopCount[i] += 1;
		}
	}
}
