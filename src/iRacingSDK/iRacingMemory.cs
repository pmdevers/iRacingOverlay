using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using Win32.Synchronization;

namespace iRacingSDK
{
	public class Defines
	{
		public const uint DesiredAccess = 2031619;
		public const string DataValidEventName = "Local\\IRSDKDataValidEvent";
		public const string MemMapFileName = "Local\\IRSDKMemMapFileName";
		public const string BroadcastMessageName = "IRSDK_BROADCASTMSG";
		public const string PadCarNumName = "IRSDK_PADCARNUM";
		public const uint ErrorFileNotFound = 2;
		public const int MaxString = 32;
		public const int MaxDesc = 64;
		public const int MaxVars = 4096;
		public const int MaxBufs = 4;
		public const int StatusConnected = 1;
		public const int SessionStringLength = 0x20000; // 128k
	}

	internal class iRacingMemory
	{
		private IntPtr _dataValidEvent;
		private MemoryMappedFile _irsdkMappedMemory;

		public MemoryMappedViewAccessor Accessor { get; private set; }
		
		public bool IsConnected()
		{
			if (Accessor != null)
				return true;

			var dataValidEvent = Event.OpenEvent(Defines.DesiredAccess, false, Defines.DataValidEventName);
			if (dataValidEvent == IntPtr.Zero)
			{
				var lastError = Marshal.GetLastWin32Error();
				if (lastError == Defines.ErrorFileNotFound)
					return false;

				Trace.WriteLine($"Unable to open event {Defines.DataValidEventName} - Error Code {lastError}", "DEBUG");
				return false;
			}

			MemoryMappedFile irsdkMappedMemory = null;
			try
			{
				irsdkMappedMemory = MemoryMappedFile.OpenExisting(Defines.MemMapFileName);
			}
			catch (Exception e)
			{
				Trace.WriteLine("Error accessing shared memory", "DEBUG");
				Trace.WriteLine(e.Message, "DEBUG");
			}

			if (irsdkMappedMemory == null)
				return false;

			var accessor = irsdkMappedMemory.CreateViewAccessor();

			_irsdkMappedMemory = irsdkMappedMemory;
			_dataValidEvent = dataValidEvent;
			Accessor = accessor;
			return true;
		}

		// TODO
		public bool WaitForData()
		{
			return Event.WaitForSingleObject(_dataValidEvent, 17) == 0;
		}
	}
}
