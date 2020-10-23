using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using iRacingSDK.Logging;

namespace iRacingSDK
{
	public class iRacingEvents : IDisposable
	{
		private readonly iRacingConnection _instance = new iRacingConnection();
		private readonly CrossThreadEvents<DataSample> _newData = new CrossThreadEvents<DataSample>();
		private readonly CrossThreadEvents<DataSample> _newSessionData = new CrossThreadEvents<DataSample>();
		private readonly CrossThreadEvents _connected = new CrossThreadEvents();
		private readonly CrossThreadEvents _disconnected = new CrossThreadEvents();
		private readonly TimeSpan _period;

		private Task _backListener;
		private bool _requestCancel;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="period">The time interval for raising the NewData event</param>
		public iRacingEvents(TimeSpan period)
		{
			_period = period;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="periodInMilliseconds">The time interval in milliseconds to raise the NewData Event.  0 means as often as data arrives from iRacing.</param>
		public iRacingEvents(int periodInMilliseconds = 0)
		{
			_period = new TimeSpan(0, 0, 0, 0, periodInMilliseconds);
		}

		public event Action Connected
		{
			add => _connected.Event += value;
			remove => _connected.Event -= value;
		}

		public event Action Disconnected
		{
			add => _disconnected.Event += value;
			remove => _disconnected.Event -= value;
		}

		public event Action<DataSample> NewData
		{
			add => _newData.Event += value;
			remove => _newData.Event -= value;
		}

		public event Action<DataSample> NewSessionData
		{
			add => _newSessionData.Event += value;
			remove => _newSessionData.Event -= value;
		}

		public void StartListening()
		{
			if (_backListener != null)
				throw new Exception("Already listening to iRacing data");

			_requestCancel = false;

			_backListener = new Task(Listen);
			_backListener.Start();
		}

		public void StopListening()
		{
			var bl = _backListener;

			if (_backListener == null)
				throw new Exception("Not currently listening to iRacing data");

			_requestCancel = true;
			bl.Wait(500);
		}

		void Listen()
		{
			var isConnected = false;
			var isDisconnected = true;
			var lastSessionInfoUpdate = -1;
			var lastTimeStamp = DateTime.Now;

			try
			{
				foreach (var d in _instance.GetDataFeed(logging: false))
				{
					if (_requestCancel)
						return;

					if (!isConnected && d.IsConnected)
					{
						isConnected = true;
						isDisconnected = false;
						_connected.Invoke();
					}

					if (!isDisconnected && !d.IsConnected)
					{
						isConnected = false;
						isDisconnected = true;
						_disconnected.Invoke();
					}

					if (_period >= (DateTime.Now - lastTimeStamp))
						continue;

					lastTimeStamp = DateTime.Now;

					if (d.IsConnected)
						_newData.Invoke(d);

					if (d.IsConnected && d.SessionData.InfoUpdate != lastSessionInfoUpdate)
					{
						lastSessionInfoUpdate = d.SessionData.InfoUpdate;
						_newSessionData.Invoke(d);
					}
				}
			}
			catch (Exception e)
			{
				TraceError.WriteLine(e.Message);
				TraceError.WriteLine(e.StackTrace);
			}
			finally
			{
				_backListener = null;
			}
		}

		public void Dispose()
		{
			StopListening();
		}
	}
}
