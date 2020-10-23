using System;

namespace Win32
{
	public static class Keyboard
	{
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

		public const int KEYEVENTF_KEYUP = 0x02;
		public const byte VK_MENU = 0x12;
		public const byte VK_F9 = 0x78;
		public const byte VK_F10 = 0x79;
		public const byte VK_TAB = 0x09;
	}
}
