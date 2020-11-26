using System;
using System.Collections.Generic;
using System.Text;

namespace iRacingSDK
{
	public partial class SessionData
	{

		public partial class _WeekendInfo
		{
			public string TrackName { get; set; }
			public int TrackID { get; set; }
			public string TrackLength { get; set; }
			public string TrackDisplayName { get; set; }
			public string TrackDisplayShortName { get; set; }
			public string TrackConfigName { get; set; }
			public string TrackCity { get; set; }
			public string TrackCountry { get; set; }
			public string TrackAltitude { get; set; }
			public string TrackLatitude { get; set; }
			public string Trackintitude { get; set; }
			public string TrackNorthOffset { get; set; }
			public int TrackNumTurns { get; set; }
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
			public int TrackCleanup { get; set; }
			public int TrackDynamicTrack { get; set; }
			public int SeriesID { get; set; }
			public int SeasonID { get; set; }
			public int SessionID { get; set; }
			public int SubSessionID { get; set; }
			public int LeagueID { get; set; }
			public int Official { get; set; }
			public int RaceWeek { get; set; }
			public string EventType { get; set; }
			public string Category { get; set; }
			public string SimMode { get; set; }
			public int TeamRacing { get; set; }
			public int MinDrivers { get; set; }
			public int MaxDrivers { get; set; }
			public string DCRuleSet { get; set; }
			public int QualifierMustStartRace { get; set; }
			public int NumCarClasses { get; set; }
			public int NumCarTypes { get; set; }

			public partial class _WeekendOptions
			{
				public int NumStarters { get; set; }
				public string StartingGrid { get; set; }
				public string QualifyScoring { get; set; }
				public string CourseCautions { get; set; }
				public int StandingStart { get; set; }
				public string Restarts { get; set; }
				public string WeatherType { get; set; }
				public string Skies { get; set; }
				public string WindDirection { get; set; }
				public string WindSpeed { get; set; }
				public string WeatherTemp { get; set; }
				public string RelativeHumidity { get; set; }
				public string FogLevel { get; set; }
				public int Unofficial { get; set; }
				public string CommercialMode { get; set; }
				public string NightMode { get; set; }
				public int IsFixedSetup { get; set; }
				public string StrictLapsChecking { get; set; }
				public int HasOpenRegistration { get; set; }
				public int HardcoreLevel { get; set; }
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
				public int SessionNumLapsToAvg { get; set; }
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
					public int CarIdx { get; set; }
					public int FastestLap { get; set; }
					public double FastestTime { get; set; }
				}

				public _ResultsFastestLap[] ResultsFastestLap { get; set; }
				public double ResultsAverageLapTime { get; set; }
				public int ResultsNumCautionFlags { get; set; }
				public int ResultsNumCautionLaps { get; set; }
				public int ResultsNumLeadChanges { get; set; }
				public int ResultsLapsComplete { get; set; }
				public int ResultsOfficial { get; set; }
			}

			public _Sessions[] Sessions { get; set; }
		}

		public _SessionInfo SessionInfo { get; set; }

		public partial class _CameraInfo
		{
			public partial class _Groups
			{
				public int GroupNum { get; set; }
				public string GroupName { get; set; }
				public partial class _Cameras
				{
					public int CameraNum { get; set; }
					public string CameraName { get; set; }
				}

				public _Cameras[] Cameras { get; set; }
			}

			public _Groups[] Groups { get; set; }
		}

		public _CameraInfo CameraInfo { get; set; }

		public partial class _RadioInfo
		{
			public int SelectedRadioNum { get; set; }
			public partial class _Radios
			{
				public int RadioNum { get; set; }
				public int HopCount { get; set; }
				public int NumFrequencies { get; set; }
				public int TunedToFrequencyNum { get; set; }
				public int ScanningIsOn { get; set; }
				public partial class _Frequencies
				{
					public int FrequencyNum { get; set; }
					public string FrequencyName { get; set; }
					public int Priority { get; set; }
					public int CarIdx { get; set; }
					public int EntryIdx { get; set; }
					public int ClubID { get; set; }
					public int CanScan { get; set; }
					public int CanSquawk { get; set; }
					public int Muted { get; set; }
					public int IsMutable { get; set; }
					public int IsDeletable { get; set; }
				}

				public _Frequencies[] Frequencies { get; set; }
			}

			public _Radios[] Radios { get; set; }
		}

		public _RadioInfo RadioInfo { get; set; }

		public partial class _DriverInfo
		{
			public int DriverCarIdx { get; set; }
			public int PaceCarIdx { get; set; }
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
			public int DriverSetupIsModified { get; set; }
			public string DriverSetupLoadTypeName { get; set; }
			public int DriverSetupPassedTech { get; set; }
			public partial class _Drivers
			{
				public int CarIdx { get; set; }
				public string UserName { get; set; }
				public string AbbrevName { get; set; }
				public string Initials { get; set; }
				public int UserID { get; set; }
				public int TeamID { get; set; }
				public string TeamName { get; set; }
				public string CarNumber { get; set; }
				public int CarNumberRaw { get; set; }
				public string CarPath { get; set; }
				public int CarClassID { get; set; }
				public int CarID { get; set; }
				public int CarIsPaceCar { get; set; }
				public int CarIsAI { get; set; }
				public string CarScreenName { get; set; }
				public string CarScreenNameShort { get; set; }
				public string CarClassShortName { get; set; }
				public int CarClassRelSpeed { get; set; }
				public int CarClassLicenseLevel { get; set; }
				public string CarClassMaxFuel { get; set; }
				public string CarClassMaxFuelPct { get; set; }
				public string CarClassWeightPenalty { get; set; }
				public string CarClassColor { get; set; }
				public int IRating { get; set; }
				public int LicLevel { get; set; }
				public int LicSubLevel { get; set; }
				public string LicString { get; set; }
				public string LicColor { get; set; }
				public int IsSpectator { get; set; }
				public string CarDesignStr { get; set; }
				public string HelmetDesignStr { get; set; }
				public string SuitDesignStr { get; set; }
				public string CarNumberDesignStr { get; set; }
				public int CarSponsor_1 { get; set; }
				public int CarSponsor_2 { get; set; }
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
				public int SectorNum { get; set; }
				public double SectorStartPct { get; set; }
			}

			public _Sectors[] Sectors { get; set; }
		}

		public _SplitTimeInfo SplitTimeInfo { get; set; }
	}
}
