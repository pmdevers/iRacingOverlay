using System;
using System.Collections.Generic;

namespace iRacingSDK
{
	public static class iRacing
	{
		static iRacingConnection instance;
		static iRacingEvents eventInstance;

		static iRacing()
		{
			instance = new iRacingConnection();
			eventInstance = new iRacingEvents();
		}

		public static Replay Replay { get { return instance.Replay; } }
		public static PitCommand PitCommand { get { return instance.PitCommand; } }

		public static bool IsConnected { get { return instance.IsConnected; } }

		public static IEnumerable<DataSample> GetDataFeed()
		{
			return instance.GetDataFeed();
		}

		public static void StartListening()
		{
			eventInstance.StartListening();
		}

		public static void StopListening()
		{
			eventInstance.StopListening();
		}

		public static event Action<DataSample> NewData
		{
			add
			{
				eventInstance.NewData += value;
			}
			remove
			{
				eventInstance.NewData -= value;
			}
		}
	}
}
