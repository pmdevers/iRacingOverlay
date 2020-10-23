using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iRacingBlazor.Models
{
	public class DriverModel
	{
		public long Id { get; set; }
		public long Position { get; set; }
		public string Name { get; set; }
		public long CustomerId { get; set; }
		public bool Selected { get; set; }
		public bool Show { get; set; }
		public bool Allow { get; set; }
		public string Number { get; set; }
		public long ClassId { get; set; }
		public long CarClassRelSpeed { get; set; }
		public long Rating { get; set; }
		public double FastestLapTime { get; set; }
		public double LastLapTime { get; set; }
	}
}
