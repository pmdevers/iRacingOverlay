namespace iRacingSDK.Data
{
	public partial class SessionData
	{

		public partial class _WeekendInfo
		{
			public string TrackName { get; set; }
			public long TrackID { get; set; }
			public string TrackLength { get; set; }
			public string TrackDisplayName { get; set; }
			public string TrackDisplayShortName { get; set; }
			public string TrackConfigName { get; set; }
			public string TrackCity { get; set; }
			public string TrackCountry { get; set; }
			public string TrackAltitude { get; set; }
			public string TrackLatitude { get; set; }
			public string TrackLongitude { get; set; }
			public string TrackNorthOffset { get; set; }
			public long TrackNumTurns { get; set; }
			public string TrackPitSpeedLimit { get; set; }
			public string TrackType { get; set; }
			public string TrackWeatherType { get; set; }
			public string TrackSkies { get; set; }
			public string TrackSurfaceTemp { get; set; }
			public string TrackAirTemp { get; set; }
			public string TrackAirPressure { get; set; }
			public string TrackWindVel { get; set; }
			public string TrackWindDir { get; set; }
			public string TrackRelativeHumidity { get; set; }
			public string TrackFogLevel { get; set; }
			public long TrackCleanup { get; set; }
			public long TrackDynamicTrack { get; set; }
			public long SeriesID { get; set; }
			public long SeasonID { get; set; }
			public long SessionID { get; set; }
			public long SubSessionID { get; set; }
			public long LeagueID { get; set; }
			public long Official { get; set; }
			public long RaceWeek { get; set; }
			public string EventType { get; set; }
			public string Category { get; set; }
			public string SimMode { get; set; }
			public long TeamRacing { get; set; }
			public long MinDrivers { get; set; }
			public long MaxDrivers { get; set; }
			public string DCRuleSet { get; set; }
			public long QualifierMustStartRace { get; set; }
			public long NumCarClasses { get; set; }
			public long NumCarTypes { get; set; }

			public partial class _WeekendOptions
			{
				public long NumStarters { get; set; }
				public string StartingGrid { get; set; }
				public string QualifyScoring { get; set; }
				public string CourseCautions { get; set; }
				public long StandingStart { get; set; }
				public string Restarts { get; set; }
				public string WeatherType { get; set; }
				public string Skies { get; set; }
				public string WindDirection { get; set; }
				public string WindSpeed { get; set; }
				public string WeatherTemp { get; set; }
				public string RelativeHumidity { get; set; }
				public string FogLevel { get; set; }
				public long Unofficial { get; set; }
				public string CommercialMode { get; set; }
				public string NightMode { get; set; }
				public long IsFixedSetup { get; set; }
				public string StrictLapsChecking { get; set; }
				public long HasOpenRegistration { get; set; }
				public long HardcoreLevel { get; set; }
			}

			public _WeekendOptions WeekendOptions { get; set; }

			public partial class _TelemetryOptions
			{
				public string TelemetryDiskFile { get; set; }
			}

			public _TelemetryOptions TelemetryOptions { get; set; }
		}

		public _WeekendInfo WeekendInfo { get; set; }

		public partial class _SessionInfo
		{
			public partial class _Sessions
			{
				public int SessionNum { get; set; }
				public string SessionLaps { get; set; }
				public string SessionTime { get; set; }
				public long SessionNumLapsToAvg { get; set; }
				public string SessionType { get; set; }
				public string SessionTrackRubberState { get; set; }
				public partial class _ResultsPositions
				{
					public int Position { get; set; }
					public int ClassPosition { get; set; }
					public int CarIdx { get; set; }
					public int Lap { get; set; }
					public double Time { get; set; }
					public int FastestLap { get; set; }
					public double FastestTime { get; set; }
					public double LastTime { get; set; }
					public int LapsLed { get; set; }
					public int LapsComplete { get; set; }
					public double LapsDriven { get; set; }
					public int Incidents { get; set; }
					public int ReasonOutId { get; set; }
					public string ReasonOutStr { get; set; }
				}

				public _ResultsPositions[] ResultsPositions { get; set; }
				public partial class _ResultsFastestLap
				{
					public long CarIdx { get; set; }
					public long FastestLap { get; set; }
					public double FastestTime { get; set; }
				}

				public _ResultsFastestLap[] ResultsFastestLap { get; set; }
				public double ResultsAverageLapTime { get; set; }
				public long ResultsNumCautionFlags { get; set; }
				public long ResultsNumCautionLaps { get; set; }
				public long ResultsNumLeadChanges { get; set; }
				public long ResultsLapsComplete { get; set; }
				public long ResultsOfficial { get; set; }
			}

			public _Sessions[] Sessions { get; set; }
		}

		public _SessionInfo SessionInfo { get; set; }

		public partial class _CameraInfo
		{
			public partial class _Groups
			{
				public long GroupNum { get; set; }
				public string GroupName { get; set; }
				public partial class _Cameras
				{
					public long CameraNum { get; set; }
					public string CameraName { get; set; }
				}

				public _Cameras[] Cameras { get; set; }
			}

			public _Groups[] Groups { get; set; }
		}

		public _CameraInfo CameraInfo { get; set; }

		public partial class _RadioInfo
		{
			public long SelectedRadioNum { get; set; }
			public partial class _Radios
			{
				public long RadioNum { get; set; }
				public long HopCount { get; set; }
				public long NumFrequencies { get; set; }
				public long TunedToFrequencyNum { get; set; }
				public long ScanningIsOn { get; set; }
				public partial class _Frequencies
				{
					public long FrequencyNum { get; set; }
					public string FrequencyName { get; set; }
					public long Priority { get; set; }
					public long CarIdx { get; set; }
					public long EntryIdx { get; set; }
					public long ClubID { get; set; }
					public long CanScan { get; set; }
					public long CanSquawk { get; set; }
					public long Muted { get; set; }
					public long IsMutable { get; set; }
					public long IsDeletable { get; set; }
				}

				public _Frequencies[] Frequencies { get; set; }
			}

			public _Radios[] Radios { get; set; }
		}

		public _RadioInfo RadioInfo { get; set; }

		public partial class _DriverInfo
		{
			public long DriverCarIdx { get; set; }
			public long PaceCarIdx { get; set; }
			public double DriverHeadPosX { get; set; }
			public double DriverHeadPosY { get; set; }
			public double DriverHeadPosZ { get; set; }
			public double DriverCarIdleRPM { get; set; }
			public double DriverCarRedLine { get; set; }
			public double DriverCarFuelKgPerLtr { get; set; }
			public double DriverCarFuelMaxLtr { get; set; }
			public double DriverCarMaxFuelPct { get; set; }
			public double DriverCarSLFirstRPM { get; set; }
			public double DriverCarSLShiftRPM { get; set; }
			public double DriverCarSLLastRPM { get; set; }
			public double DriverCarSLBlinkRPM { get; set; }
			public double DriverPitTrkPct { get; set; }
			public double DriverCarEstLapTime { get; set; }
			public string DriverSetupName { get; set; }
			public long DriverSetupIsModified { get; set; }
			public string DriverSetupLoadTypeName { get; set; }
			public long DriverSetupPassedTech { get; set; }
			public partial class _Drivers
			{
				public long CarIdx { get; set; }
				public string UserName { get; set; }
				public string AbbrevName { get; set; }
				public string Initials { get; set; }
				public int UserID { get; set; }
				public int TeamID { get; set; }
				public string TeamName { get; set; }
				public string CarNumber { get; set; }
				public long CarNumberRaw { get; set; }
				public string CarPath { get; set; }
				public int CarClassID { get; set; }
				public int CarID { get; set; }
				public int CarIsPaceCar { get; set; }
				public int CarIsAI { get; set; }
				public string CarScreenName { get; set; }
				public string CarScreenNameShort { get; set; }
				public string CarClassShortName { get; set; }
				public long CarClassRelSpeed { get; set; }
				public long CarClassLicenseLevel { get; set; }
				public string CarClassMaxFuel { get; set; }
				public string CarClassMaxFuelPct { get; set; }
				public string CarClassWeightPenalty { get; set; }
				public string CarClassColor { get; set; }
				public int IRating { get; set; }
				public int LicLevel { get; set; }
				public int LicSubLevel { get; set; }
				public string LicString { get; set; }
				public string LicColor { get; set; }
				public long IsSpectator { get; set; }
				public string CarDesignStr { get; set; }
				public string HelmetDesignStr { get; set; }
				public string SuitDesignStr { get; set; }
				public string CarNumberDesignStr { get; set; }
				public long CarSponsor_1 { get; set; }
				public long CarSponsor_2 { get; set; }
				public string ClubName { get; set; }
				public string DivisionName { get; set; }
			}

			public _Drivers[] Drivers { get; set; }
		}

		public _DriverInfo DriverInfo { get; set; }

		public partial class _SplitTimeInfo
		{
			public partial class _Sectors
			{
				public long SectorNum { get; set; }
				public double SectorStartPct { get; set; }
			}

			public _Sectors[] Sectors { get; set; }
		}

		public _SplitTimeInfo SplitTimeInfo { get; set; }
	}
}
