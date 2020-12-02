using System;

namespace iRacingSDK.Data
{
	public struct LapSector
	{
		public readonly int LapNumber;
		public readonly int Sector;

		public LapSector(int lapNumber, int sector)
		{
			Sector = sector;
			LapNumber = lapNumber;
		}

		public static LapSector ForLap(int lapNumber)
		{
			return new LapSector(lapNumber, 0);
		}

		public override bool Equals(Object obj)
		{
			return obj is LapSector && this == (LapSector)obj;
		}

		public override int GetHashCode()
		{
			return LapNumber << 4 + Sector;
		}

		public static bool operator ==(LapSector x, LapSector y)
		{
			return x.LapNumber == y.LapNumber && x.Sector == y.Sector;
		}

		public static bool operator !=(LapSector x, LapSector y)
		{
			return !(x == y);
		}

		public static bool operator >=(LapSector x, LapSector y)
		{
			if (x.LapNumber > y.LapNumber)
				return true;

			if (x.LapNumber == y.LapNumber && x.Sector >= y.Sector)
				return true;

			return false;
		}

		public static bool operator <=(LapSector x, LapSector y)
		{
			return y >= x;
		}

		public override string ToString()
		{
			return string.Format("Lap: {0}, Sector: {1}", LapNumber, Sector);
		}
	}
}
