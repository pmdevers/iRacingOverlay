using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace iRacingSDK
{
	public class DataFeed
	{
		private readonly MemoryMappedViewAccessor _accessor;

		public DataFeed(MemoryMappedViewAccessor accessor)
		{
			_accessor = accessor;
		}
	}
}
