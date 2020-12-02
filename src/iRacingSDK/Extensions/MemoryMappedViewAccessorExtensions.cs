using System.IO.MemoryMappedFiles;

namespace iRacingSDK
{
    internal static class MemoryMappedViewAccessorExtensions
	{
		public unsafe delegate T MyFn<out T>(byte* ptr);

		public static unsafe T AcquirePointer<T>(this MemoryMappedViewAccessor self, MyFn<T> fn)
		{
			byte* ptr = null;
			self.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
			try
			{
				return fn(ptr);
			}
			finally
			{
				self.SafeMemoryMappedViewHandle.ReleasePointer();
			}
		}
	}
}
