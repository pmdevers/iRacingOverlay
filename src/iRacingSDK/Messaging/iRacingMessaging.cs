using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace iRacingSDK.Messaging
{
    public class iRacingMessaging
	{
        private const double MessageThrottleTime = 1000;

		protected int MessageId { get; }
        private DateTime _lastMessagePostedTime = DateTime.Now;
        private Task _currentMessageTask;
		
		protected iRacingMessaging()
		{
			MessageId = Win32.Messages.RegisterWindowMessage("IRSDK_BROADCASTMSG");
			_currentMessageTask = new Task(() => { });
			_currentMessageTask.Start();
		}

		protected virtual void SendMessage(BroadcastMessage message, int var1 = 0, int var2 = 0)
		{
			var msgVar1 = FromShorts((short)message, var1);

			var lastTask = _currentMessageTask;
			_currentMessageTask = new Task(() =>
			{
				lastTask.Wait();
				lastTask.Dispose();
				lastTask = null;

				var timeSinceLastMsg = DateTime.Now - _lastMessagePostedTime;
				var throttleTime = (int)(MessageThrottleTime - timeSinceLastMsg.TotalMilliseconds);
				if (throttleTime > 0)
				{
					Trace.WriteLine($"Throttle message {message} delivery to iRacing by {throttleTime} millisecond", "DEBUG");
					Thread.Sleep(throttleTime);
				}
				_lastMessagePostedTime = DateTime.Now;

				if (!Win32.Messages.SendNotifyMessage(Win32.Messages.HWND_BROADCAST, MessageId, msgVar1, var2))
					throw new Exception($"Error in broadcasting message {message}");
			});

			_currentMessageTask.Start();
		}

		protected void SendMessage(BroadcastMessage message, int var1, int var2, int var3)
		{
			var var23 = FromShorts(var2, var3);
			SendMessage(message, var1, var23);
		}

		protected static int FromShorts(int lowPart, int highPart)
		{
			return (highPart << 16) | (ushort)lowPart;
		}

		public virtual void Wait()
		{
			_currentMessageTask.Wait();
		}
	}
}
