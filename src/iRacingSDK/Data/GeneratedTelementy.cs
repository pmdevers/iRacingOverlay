using System.Collections.Generic;
using iRacingSDK.Messaging;

namespace iRacingSDK.Data
{
    public partial class Telemetry : Dictionary<string, object>
    {
        public SessionData SessionData { get; set; }

        /// <summary>
        ///     Seconds since session start
        /// </summary>
        public double SessionTime => (double) this["SessionTime"];

        /// <summary>
        ///     Session number
        /// </summary>
        public int SessionNum => (int) this["SessionNum"];

        /// <summary>
        ///     Session state
        /// </summary>
        public SessionState SessionState => (SessionState) this["SessionState"];

        /// <summary>
        ///     Session ID
        /// </summary>
        public int SessionUniqueID => (int) this["SessionUniqueID"];

        /// <summary>
        ///     Session flags
        /// </summary>
        public SessionFlags SessionFlags => (SessionFlags) (int) this["SessionFlags"];

        /// <summary>
        ///     Seconds left till session ends
        /// </summary>
        public Time SessionTimeRemain => (double) this["SessionTimeRemain"];

        public int SessionLaps => (int) this["SessionLaps"];

        /// <summary>
        ///     Old laps left till session ends use SessionLapsRemainEx
        /// </summary>
        public int SessionLapsRemain => (int) this["SessionLapsRemain"];

        /// <summary>
        ///     New improved laps left till session ends
        /// </summary>
        public int SessionLapsRemainEx => (int) this["SessionLapsRemainEx"];

        /// <summary>
        ///     The car index of the current person speaking on the radio
        /// </summary>
        public int RadioTransmitCarIdx => (int) this["RadioTransmitCarIdx"];

        /// <summary>
        ///     The radio index of the current person speaking on the radio
        /// </summary>
        public int RadioTransmitRadioIdx => (int) this["RadioTransmitRadioIdx"];

        /// <summary>
        ///     The frequency index of the current person speaking on the radio
        /// </summary>
        public int RadioTransmitFrequencyIdx => (int) this["RadioTransmitFrequencyIdx"];

        /// <summary>
        ///     Default units for the user interface 0 = english 1 = metric
        /// </summary>
        public DisplayUnits DisplayUnits => (DisplayUnits) (int) this["DisplayUnits"];

        /// <summary>
        ///     Driver activated flag
        /// </summary>
        public bool DriverMarker => (bool) this["DriverMarker"];

        /// <summary>
        ///     1=Car on track physics running with player in car
        /// </summary>
        public bool IsOnTrack => (bool) this["IsOnTrack"];

        /// <summary>
        ///     0=replay not playing  1=replay playing
        /// </summary>
        public bool IsReplayPlaying => (bool) this["IsReplayPlaying"];

        /// <summary>
        ///     Integer replay frame number (60 per second)
        /// </summary>
        public int ReplayFrameNum => (int) this["ReplayFrameNum"];

        /// <summary>
        ///     Integer replay frame number from end of tape
        /// </summary>
        public int ReplayFrameNumEnd => (int) this["ReplayFrameNumEnd"];

        /// <summary>
        ///     0=disk based telemetry turned off  1=turned on
        /// </summary>
        public bool IsDiskLoggingEnabled => (bool) this["IsDiskLoggingEnabled"];

        /// <summary>
        ///     0=disk based telemetry file not being written  1=being written
        /// </summary>
        public bool IsDiskLoggingActive => (bool) this["IsDiskLoggingActive"];

        /// <summary>
        ///     Average frames per second
        /// </summary>
        public float FrameRate => (float) this["FrameRate"];

        /// <summary>
        ///     Percent of available tim bg thread took with a 1 sec avg
        /// </summary>
        public float CpuUsageBG => (float) this["CpuUsageBG"];

        /// <summary>
        ///     Players position in race
        /// </summary>
        public int PlayerCarPosition => (int) this["PlayerCarPosition"];

        /// <summary>
        ///     Players class position in race
        /// </summary>
        public int PlayerCarClassPosition => (int) this["PlayerCarClassPosition"];

        /// <summary>
        ///     Laps started by car index
        /// </summary>
        public int[] CarIdxLap => (int[]) this["CarIdxLap"];

        /// <summary>
        ///     Laps completed by car index
        /// </summary>
        public int[] CarIdxLapCompleted => (int[]) this["CarIdxLapCompleted"];

        /// <summary>
        ///     Percentage distance around lap by car index
        /// </summary>
        public float[] CarIdxLapDistPct => (float[]) this["CarIdxLapDistPct"];

        /// <summary>
        ///     Track surface type by car index
        /// </summary>
        public TrackLocation[] CarIdxTrackSurface => (TrackLocation[]) this["CarIdxTrackSurface"];

        /// <summary>
        ///     On pit road between the cones by car index
        /// </summary>
        public bool[] CarIdxOnPitRoad => (bool[]) this["CarIdxOnPitRoad"];

        /// <summary>
        ///     Cars position in race by car index
        /// </summary>
        public int[] CarIdxPosition => (int[]) this["CarIdxPosition"];

        /// <summary>
        ///     Cars class position in race by car index
        /// </summary>
        public int[] CarIdxClassPosition => (int[]) this["CarIdxClassPosition"];

        /// <summary>
        ///     Race time behind leader or fastest lap time otherwise
        /// </summary>
        public float[] CarIdxF2Time => (float[]) this["CarIdxF2Time"];

        /// <summary>
        ///     Estimated time to reach current location on track
        /// </summary>
        public float[] CarIdxEstTime => (float[]) this["CarIdxEstTime"];

        /// <summary>
        ///     Is the player car on pit road between the cones
        /// </summary>
        public bool OnPitRoad => (bool) this["OnPitRoad"];

        /// <summary>
        ///     Steering wheel angle by car index
        /// </summary>
        public float[] CarIdxSteer => (float[]) this["CarIdxSteer"];

        /// <summary>
        ///     Engine rpm by car index
        /// </summary>
        public float[] CarIdxRPM => (float[]) this["CarIdxRPM"];

        /// <summary>
        ///     -1=reverse  0=neutral  1..n=current gear by car index
        /// </summary>
        public int[] CarIdxGear => (int[]) this["CarIdxGear"];

        /// <summary>
        ///     Steering wheel angle
        /// </summary>
        public float SteeringWheelAngle => (float) this["SteeringWheelAngle"];

        /// <summary>
        ///     0=off throttle to 1=full throttle
        /// </summary>
        public float Throttle => (float) this["Throttle"];

        /// <summary>
        ///     0=brake released to 1=max pedal force
        /// </summary>
        public float Brake => (float) this["Brake"];

        /// <summary>
        ///     0=disengaged to 1=fully engaged
        /// </summary>
        public float Clutch => (float) this["Clutch"];

        /// <summary>
        ///     -1=reverse  0=neutral  1..n=current gear
        /// </summary>
        public int Gear => (int) this["Gear"];

        /// <summary>
        ///     Engine rpm
        /// </summary>
        public float RPM => (float) this["RPM"];

        /// <summary>
        ///     Laps started count
        /// </summary>
        public int Lap => (int) this["Lap"];

        /// <summary>
        ///     Laps completed count
        /// </summary>
        public int LapCompleted => (int) this["LapCompleted"];

        /// <summary>
        ///     Meters traveled from S/F this lap
        /// </summary>
        public float LapDist => (float) this["LapDist"];

        /// <summary>
        ///     Percentage distance around lap
        /// </summary>
        public float LapDistPct => (float) this["LapDistPct"];

        /// <summary>
        ///     Laps completed in race
        /// </summary>
        public int RaceLaps => (int) this["RaceLaps"];

        /// <summary>
        ///     Players best lap number
        /// </summary>
        public int LapBestLap => (int) this["LapBestLap"];

        /// <summary>
        ///     Players best lap time
        /// </summary>
        public float LapBestLapTime => (float) this["LapBestLapTime"];

        /// <summary>
        ///     Players last lap time
        /// </summary>
        public float LapLastLapTime => (float) this["LapLastLapTime"];

        /// <summary>
        ///     Estimate of players current lap time as shown in F3 box
        /// </summary>
        public float LapCurrentLapTime => (float) this["LapCurrentLapTime"];

        /// <summary>
        ///     Player num consecutive clean laps completed for N average
        /// </summary>
        public int LapLasNLapSeq => (int) this["LapLasNLapSeq"];

        /// <summary>
        ///     Player last N average lap time
        /// </summary>
        public float LapLastNLapTime => (float) this["LapLastNLapTime"];

        /// <summary>
        ///     Player last lap in best N average lap time
        /// </summary>
        public int LapBestNLapLap => (int) this["LapBestNLapLap"];

        /// <summary>
        ///     Player best N average lap time
        /// </summary>
        public float LapBestNLapTime => (float) this["LapBestNLapTime"];

        /// <summary>
        ///     Delta time for best lap
        /// </summary>
        public float LapDeltaToBestLap => (float) this["LapDeltaToBestLap"];

        /// <summary>
        ///     Rate of change of delta time for best lap
        /// </summary>
        public float LapDeltaToBestLap_DD => (float) this["LapDeltaToBestLap_DD"];

        /// <summary>
        ///     Delta time for best lap is valid
        /// </summary>
        public bool LapDeltaToBestLap_OK => (bool) this["LapDeltaToBestLap_OK"];

        /// <summary>
        ///     Delta time for optimal lap
        /// </summary>
        public float LapDeltaToOptimalLap => (float) this["LapDeltaToOptimalLap"];

        /// <summary>
        ///     Rate of change of delta time for optimal lap
        /// </summary>
        public float LapDeltaToOptimalLap_DD => (float) this["LapDeltaToOptimalLap_DD"];

        /// <summary>
        ///     Delta time for optimal lap is valid
        /// </summary>
        public bool LapDeltaToOptimalLap_OK => (bool) this["LapDeltaToOptimalLap_OK"];

        /// <summary>
        ///     Delta time for session best lap
        /// </summary>
        public float LapDeltaToSessionBestLap => (float) this["LapDeltaToSessionBestLap"];

        /// <summary>
        ///     Rate of change of delta time for session best lap
        /// </summary>
        public float LapDeltaToSessionBestLap_DD => (float) this["LapDeltaToSessionBestLap_DD"];

        /// <summary>
        ///     Delta time for session best lap is valid
        /// </summary>
        public bool LapDeltaToSessionBestLap_OK => (bool) this["LapDeltaToSessionBestLap_OK"];

        /// <summary>
        ///     Delta time for session optimal lap
        /// </summary>
        public float LapDeltaToSessionOptimalLap => (float) this["LapDeltaToSessionOptimalLap"];

        /// <summary>
        ///     Rate of change of delta time for session optimal lap
        /// </summary>
        public float LapDeltaToSessionOptimalLap_DD => (float) this["LapDeltaToSessionOptimalLap_DD"];

        /// <summary>
        ///     Delta time for session optimal lap is valid
        /// </summary>
        public bool LapDeltaToSessionOptimalLap_OK => (bool) this["LapDeltaToSessionOptimalLap_OK"];

        /// <summary>
        ///     Delta time for session last lap
        /// </summary>
        public float LapDeltaToSessionLastlLap => (float) this["LapDeltaToSessionLastlLap"];

        /// <summary>
        ///     Rate of change of delta time for session last lap
        /// </summary>
        public float LapDeltaToSessionLastlLap_DD => (float) this["LapDeltaToSessionLastlLap_DD"];

        /// <summary>
        ///     Delta time for session last lap is valid
        /// </summary>
        public bool LapDeltaToSessionLastlLap_OK => (bool) this["LapDeltaToSessionLastlLap_OK"];

        /// <summary>
        ///     Longitudinal acceleration (including gravity)
        /// </summary>
        public float LongAccel => (float) this["LongAccel"];

        /// <summary>
        ///     Lateral acceleration (including gravity)
        /// </summary>
        public float LatAccel => (float) this["LatAccel"];

        /// <summary>
        ///     Vertical acceleration (including gravity)
        /// </summary>
        public float VertAccel => (float) this["VertAccel"];

        /// <summary>
        ///     Roll rate
        /// </summary>
        public float RollRate => (float) this["RollRate"];

        /// <summary>
        ///     Pitch rate
        /// </summary>
        public float PitchRate => (float) this["PitchRate"];

        /// <summary>
        ///     Yaw rate
        /// </summary>
        public float YawRate => (float) this["YawRate"];

        /// <summary>
        ///     GPS vehicle speed
        /// </summary>
        public float Speed => (float) this["Speed"];

        /// <summary>
        ///     X velocity
        /// </summary>
        public float VelocityX => (float) this["VelocityX"];

        /// <summary>
        ///     Y velocity
        /// </summary>
        public float VelocityY => (float) this["VelocityY"];

        /// <summary>
        ///     Z velocity
        /// </summary>
        public float VelocityZ => (float) this["VelocityZ"];

        /// <summary>
        ///     Yaw orientation
        /// </summary>
        public float Yaw => (float) this["Yaw"];

        /// <summary>
        ///     Yaw orientation relative to north
        /// </summary>
        public float YawNorth => (float) this["YawNorth"];

        /// <summary>
        ///     Pitch orientation
        /// </summary>
        public float Pitch => (float) this["Pitch"];

        /// <summary>
        ///     Roll orientation
        /// </summary>
        public float Roll => (float) this["Roll"];

        /// <summary>
        ///     Indicate action the reset key will take 0 enter 1 exit 2 reset
        /// </summary>
        public int EnterExitReset => (int) this["EnterExitReset"];

        /// <summary>
        ///     Temperature of track at start/finish line
        /// </summary>
        public float TrackTemp => (float) this["TrackTemp"];

        /// <summary>
        ///     Temperature of track measured by crew around track
        /// </summary>
        public float TrackTempCrew => (float) this["TrackTempCrew"];

        /// <summary>
        ///     Temperature of air at start/finish line
        /// </summary>
        public float AirTemp => (float) this["AirTemp"];

        /// <summary>
        ///     Weather type (0=constant  1=dynamic)
        /// </summary>
        public WeatherType WeatherType => (WeatherType) (int) this["WeatherType"];

        /// <summary>
        ///     Skies (0=clear/1=p cloudy/2=m cloudy/3=overcast)
        /// </summary>
        public Skies Skies => (Skies) (int) this["Skies"];

        /// <summary>
        ///     Density of air at start/finish line
        /// </summary>
        public float AirDensity => (float) this["AirDensity"];

        /// <summary>
        ///     Pressure of air at start/finish line
        /// </summary>
        public float AirPressure => (float) this["AirPressure"];

        /// <summary>
        ///     Wind velocity at start/finish line
        /// </summary>
        public float WindVel => (float) this["WindVel"];

        /// <summary>
        ///     Wind direction at start/finish line
        /// </summary>
        public float WindDir => (float) this["WindDir"];

        /// <summary>
        ///     Relative Humidity
        /// </summary>
        public float RelativeHumidity => (float) this["RelativeHumidity"];

        /// <summary>
        ///     Fog level
        /// </summary>
        public float FogLevel => (float) this["FogLevel"];

        /// <summary>
        ///     Status of driver change lap requirements
        /// </summary>
        public int DCLapStatus => (int) this["DCLapStatus"];

        /// <summary>
        ///     Number of team drivers who have run a stint
        /// </summary>
        public int DCDriversSoFar => (int) this["DCDriversSoFar"];

        /// <summary>
        ///     True if it is ok to reload car textures at this time
        /// </summary>
        public bool OkToReloadTextures => (bool) this["OkToReloadTextures"];

        /// <summary>
        ///     Time left for mandatory pit repairs if repairs are active
        /// </summary>
        public float PitRepairLeft => (float) this["PitRepairLeft"];

        /// <summary>
        ///     Time left for optional repairs if repairs are active
        /// </summary>
        public float PitOptRepairLeft => (float) this["PitOptRepairLeft"];

        /// <summary>
        ///     Active camera's focus car index
        /// </summary>
        public int CamCarIdx => (int) this["CamCarIdx"];

        /// <summary>
        ///     Active camera number
        /// </summary>
        public int CamCameraNumber => (int) this["CamCameraNumber"];

        /// <summary>
        ///     Active camera group number
        /// </summary>
        public int CamGroupNumber => (int) this["CamGroupNumber"];

        /// <summary>
        ///     State of camera system
        /// </summary>
        public int CamCameraState => (int) this["CamCameraState"];

        /// <summary>
        ///     1=Car on track physics running
        /// </summary>
        public bool IsOnTrackCar => (bool) this["IsOnTrackCar"];

        /// <summary>
        ///     1=Car in garage physics running
        /// </summary>
        public bool IsInGarage => (bool) this["IsInGarage"];

        /// <summary>
        ///     Output torque on steering shaft
        /// </summary>
        public float SteeringWheelTorque => (float) this["SteeringWheelTorque"];

        /// <summary>
        ///     Force feedback % max torque on steering shaft unsigned
        /// </summary>
        public float SteeringWheelPctTorque => (float) this["SteeringWheelPctTorque"];

        /// <summary>
        ///     Force feedback % max torque on steering shaft signed
        /// </summary>
        public float SteeringWheelPctTorqueSign => (float) this["SteeringWheelPctTorqueSign"];

        /// <summary>
        ///     Force feedback % max torque on steering shaft signed stops
        /// </summary>
        public float SteeringWheelPctTorqueSignStops => (float) this["SteeringWheelPctTorqueSignStops"];

        /// <summary>
        ///     Force feedback % max damping
        /// </summary>
        public float SteeringWheelPctDamper => (float) this["SteeringWheelPctDamper"];

        /// <summary>
        ///     Steering wheel max angle
        /// </summary>
        public float SteeringWheelAngleMax => (float) this["SteeringWheelAngleMax"];

        /// <summary>
        ///     DEPRECATED use DriverCarSLBlinkRPM instead
        /// </summary>
        public float ShiftIndicatorPct => (float) this["ShiftIndicatorPct"];

        /// <summary>
        ///     Friction torque applied to gears when shifting or grinding
        /// </summary>
        public float ShiftPowerPct => (float) this["ShiftPowerPct"];

        /// <summary>
        ///     RPM of shifter grinding noise
        /// </summary>
        public float ShiftGrindRPM => (float) this["ShiftGrindRPM"];

        /// <summary>
        ///     Raw throttle input 0=off throttle to 1=full throttle
        /// </summary>
        public float ThrottleRaw => (float) this["ThrottleRaw"];

        /// <summary>
        ///     Raw brake input 0=brake released to 1=max pedal force
        /// </summary>
        public float BrakeRaw => (float) this["BrakeRaw"];

        /// <summary>
        ///     Peak torque mapping to direct input units for FFB
        /// </summary>
        public float SteeringWheelPeakForceNm => (float) this["SteeringWheelPeakForceNm"];

        /// <summary>
        ///     Bitfield for warning lights
        /// </summary>
        public EngineWarnings EngineWarnings => (EngineWarnings) (int) this["EngineWarnings"];

        /// <summary>
        ///     Liters of fuel remaining
        /// </summary>
        public float FuelLevel => (float) this["FuelLevel"];

        /// <summary>
        ///     Percent fuel remaining
        /// </summary>
        public float FuelLevelPct => (float) this["FuelLevelPct"];

        /// <summary>
        ///     Bitfield of pit service checkboxes
        /// </summary>
        public int PitSvFlags => (int) this["PitSvFlags"];

        /// <summary>
        ///     Pit service left front tire pressure
        /// </summary>
        public float PitSvLFP => (float) this["PitSvLFP"];

        /// <summary>
        ///     Pit service right front tire pressure
        /// </summary>
        public float PitSvRFP => (float) this["PitSvRFP"];

        /// <summary>
        ///     Pit service left rear tire pressure
        /// </summary>
        public float PitSvLRP => (float) this["PitSvLRP"];

        /// <summary>
        ///     Pit service right rear tire pressure
        /// </summary>
        public float PitSvRRP => (float) this["PitSvRRP"];

        /// <summary>
        ///     Pit service fuel add amount
        /// </summary>
        public float PitSvFuel => (float) this["PitSvFuel"];

        /// <summary>
        ///     Replay playback speed
        /// </summary>
        public int ReplayPlaySpeed => (int) this["ReplayPlaySpeed"];

        /// <summary>
        ///     0=not slow motion  1=replay is in slow motion
        /// </summary>
        public bool ReplayPlaySlowMotion => (bool) this["ReplayPlaySlowMotion"];

        /// <summary>
        ///     Seconds since replay session start
        /// </summary>
        public double ReplaySessionTime => (double) this["ReplaySessionTime"];

        /// <summary>
        ///     Replay session number
        /// </summary>
        public int ReplaySessionNum => (int) this["ReplaySessionNum"];

        /// <summary>
        ///     In car front anti roll bar adjustment
        /// </summary>
        public float dcAntiRollFront => (float) this["dcAntiRollFront"];

        /// <summary>
        ///     In car brake bias adjustment
        /// </summary>
        public float dcBrakeBias => (float) this["dcBrakeBias"];

        /// <summary>
        ///     In car traction control adjustment
        /// </summary>
        public float dcTractionControl => (float) this["dcTractionControl"];

        /// <summary>
        ///     In car abs adjustment
        /// </summary>
        public float dcABS => (float) this["dcABS"];

        /// <summary>
        ///     In car throttle shape adjustment
        /// </summary>
        public float dcThrottleShape => (float) this["dcThrottleShape"];

        /// <summary>
        ///     In car fuel mixture adjustment
        /// </summary>
        public float dcFuelMixture => (float) this["dcFuelMixture"];

        /// <summary>
        ///     Pitstop qtape adjustment
        /// </summary>
        public float dpQtape => (float) this["dpQtape"];

        /// <summary>
        ///     Pitstop wedge adjustment
        /// </summary>
        public float dpWedgeAdj => (float) this["dpWedgeAdj"];

        /// <summary>
        ///     In car rear anti roll bar adjustment
        /// </summary>
        public float dcAntiRollRear => (float) this["dcAntiRollRear"];

        /// <summary>
        ///     Pitstop rear wing adjustment
        /// </summary>
        public float dpRWingSetting => (float) this["dpRWingSetting"];

        /// <summary>
        ///     Engine coolant temp
        /// </summary>
        public float WaterTemp => (float) this["WaterTemp"];

        /// <summary>
        ///     Engine coolant level
        /// </summary>
        public float WaterLevel => (float) this["WaterLevel"];

        /// <summary>
        ///     Engine fuel pressure
        /// </summary>
        public float FuelPress => (float) this["FuelPress"];

        /// <summary>
        ///     Engine fuel used instantaneous
        /// </summary>
        public float FuelUsePerHour => (float) this["FuelUsePerHour"];

        /// <summary>
        ///     Engine oil temperature
        /// </summary>
        public float OilTemp => (float) this["OilTemp"];

        /// <summary>
        ///     Engine oil pressure
        /// </summary>
        public float OilPress => (float) this["OilPress"];

        /// <summary>
        ///     Engine oil level
        /// </summary>
        public float OilLevel => (float) this["OilLevel"];

        /// <summary>
        ///     Engine voltage
        /// </summary>
        public float Voltage => (float) this["Voltage"];

        /// <summary>
        ///     Engine manifold pressure
        /// </summary>
        public float ManifoldPress => (float) this["ManifoldPress"];

        /// <summary>
        ///     RR brake line pressure
        /// </summary>
        public float RRbrakeLinePress => (float) this["RRbrakeLinePress"];

        /// <summary>
        ///     RR tire cold pressure  as set in the garage
        /// </summary>
        public float RRcoldPressure => (float) this["RRcoldPressure"];

        /// <summary>
        ///     RR tire left carcass temperature
        /// </summary>
        public float RRtempCL => (float) this["RRtempCL"];

        /// <summary>
        ///     RR tire middle carcass temperature
        /// </summary>
        public float RRtempCM => (float) this["RRtempCM"];

        /// <summary>
        ///     RR tire right carcass temperature
        /// </summary>
        public float RRtempCR => (float) this["RRtempCR"];

        /// <summary>
        ///     RR tire left percent tread remaining
        /// </summary>
        public float RRwearL => (float) this["RRwearL"];

        /// <summary>
        ///     RR tire middle percent tread remaining
        /// </summary>
        public float RRwearM => (float) this["RRwearM"];

        /// <summary>
        ///     RR tire right percent tread remaining
        /// </summary>
        public float RRwearR => (float) this["RRwearR"];

        /// <summary>
        ///     LR brake line pressure
        /// </summary>
        public float LRbrakeLinePress => (float) this["LRbrakeLinePress"];

        /// <summary>
        ///     LR tire cold pressure  as set in the garage
        /// </summary>
        public float LRcoldPressure => (float) this["LRcoldPressure"];

        /// <summary>
        ///     LR tire left carcass temperature
        /// </summary>
        public float LRtempCL => (float) this["LRtempCL"];

        /// <summary>
        ///     LR tire middle carcass temperature
        /// </summary>
        public float LRtempCM => (float) this["LRtempCM"];

        /// <summary>
        ///     LR tire right carcass temperature
        /// </summary>
        public float LRtempCR => (float) this["LRtempCR"];

        /// <summary>
        ///     LR tire left percent tread remaining
        /// </summary>
        public float LRwearL => (float) this["LRwearL"];

        /// <summary>
        ///     LR tire middle percent tread remaining
        /// </summary>
        public float LRwearM => (float) this["LRwearM"];

        /// <summary>
        ///     LR tire right percent tread remaining
        /// </summary>
        public float LRwearR => (float) this["LRwearR"];

        /// <summary>
        ///     RF brake line pressure
        /// </summary>
        public float RFbrakeLinePress => (float) this["RFbrakeLinePress"];

        /// <summary>
        ///     RF tire cold pressure  as set in the garage
        /// </summary>
        public float RFcoldPressure => (float) this["RFcoldPressure"];

        /// <summary>
        ///     RF tire left carcass temperature
        /// </summary>
        public float RFtempCL => (float) this["RFtempCL"];

        /// <summary>
        ///     RF tire middle carcass temperature
        /// </summary>
        public float RFtempCM => (float) this["RFtempCM"];

        /// <summary>
        ///     RF tire right carcass temperature
        /// </summary>
        public float RFtempCR => (float) this["RFtempCR"];

        /// <summary>
        ///     RF tire left percent tread remaining
        /// </summary>
        public float RFwearL => (float) this["RFwearL"];

        /// <summary>
        ///     RF tire middle percent tread remaining
        /// </summary>
        public float RFwearM => (float) this["RFwearM"];

        /// <summary>
        ///     RF tire right percent tread remaining
        /// </summary>
        public float RFwearR => (float) this["RFwearR"];

        /// <summary>
        ///     LF brake line pressure
        /// </summary>
        public float LFbrakeLinePress => (float) this["LFbrakeLinePress"];

        /// <summary>
        ///     LF tire cold pressure  as set in the garage
        /// </summary>
        public float LFcoldPressure => (float) this["LFcoldPressure"];

        /// <summary>
        ///     LF tire left carcass temperature
        /// </summary>
        public float LFtempCL => (float) this["LFtempCL"];

        /// <summary>
        ///     LF tire middle carcass temperature
        /// </summary>
        public float LFtempCM => (float) this["LFtempCM"];

        /// <summary>
        ///     LF tire right carcass temperature
        /// </summary>
        public float LFtempCR => (float) this["LFtempCR"];

        /// <summary>
        ///     LF tire left percent tread remaining
        /// </summary>
        public float LFwearL => (float) this["LFwearL"];

        /// <summary>
        ///     LF tire middle percent tread remaining
        /// </summary>
        public float LFwearM => (float) this["LFwearM"];

        /// <summary>
        ///     LF tire right percent tread remaining
        /// </summary>
        public float LFwearR => (float) this["LFwearR"];

        /// <summary>
        ///     RR shock deflection
        /// </summary>
        public float RRshockDefl => (float) this["RRshockDefl"];

        /// <summary>
        ///     RR shock velocity
        /// </summary>
        public float RRshockVel => (float) this["RRshockVel"];

        /// <summary>
        ///     LR shock deflection
        /// </summary>
        public float LRshockDefl => (float) this["LRshockDefl"];

        /// <summary>
        ///     LR shock velocity
        /// </summary>
        public float LRshockVel => (float) this["LRshockVel"];

        /// <summary>
        ///     RF shock deflection
        /// </summary>
        public float RFshockDefl => (float) this["RFshockDefl"];

        /// <summary>
        ///     RF shock velocity
        /// </summary>
        public float RFshockVel => (float) this["RFshockVel"];

        /// <summary>
        ///     LF shock deflection
        /// </summary>
        public float LFshockDefl => (float) this["LFshockDefl"];

        /// <summary>
        ///     LF shock velocity
        /// </summary>
        public float LFshockVel => (float) this["LFshockVel"];

        /// <summary>
        ///     RRSH shock deflection
        /// </summary>
        public float RRSHshockDefl => (float) this["RRSHshockDefl"];

        /// <summary>
        ///     LRSH shock deflection
        /// </summary>
        public float LRSHshockDefl => (float) this["LRSHshockDefl"];

        /// <summary>
        ///     RFSH shock deflection
        /// </summary>
        public float RFSHshockDefl => (float) this["RFSHshockDefl"];

        /// <summary>
        ///     LFSH shock deflection
        /// </summary>
        public float LFSHshockDefl => (float) this["LFSHshockDefl"];

        /// <summary>
        /// </summary>
        public int TickCount => (int) this["TickCount"];


        public int PlayerCarIdx => (int) this["PlayerCarIdx"];
    }
}