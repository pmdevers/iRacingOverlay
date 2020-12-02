using System.Runtime.InteropServices;

namespace iRacingSDK
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct VarBuf
	{
		public int tickCount;
		public int bufOffset;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] pad;
	}
}
