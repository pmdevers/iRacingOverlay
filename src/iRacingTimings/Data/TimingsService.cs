using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;

namespace iRacingTimings.Data
{
    public class TimingsService : ISubService
    {
        private int _currentStateNumber;
        private SessionData._DriverInfo._Drivers[] _drivers = new SessionData._DriverInfo._Drivers[0];
        private SessionData._SplitTimeInfo._Sectors[] _sectors = new SessionData._SplitTimeInfo._Sectors[0];
        ConcurrentDictionary<long, List<double>> _laptimes = new ConcurrentDictionary<long, List<double>>();

        ConcurrentDictionary<long, List<double>> _stindRecord = new ConcurrentDictionary<long, List<double>>();
        ConcurrentDictionary<long, int> _currentLap = new ConcurrentDictionary<long, int>();
        ConcurrentDictionary<long, double> _currentLapStartTime = new ConcurrentDictionary<long, double>();
        ConcurrentDictionary<long, string> _gapInFront = new ConcurrentDictionary<long, string>();

        public List<TimingModel> Timings { get; set; } = new List<TimingModel>();

        public void Update(DataSample data)
        {
            UpdateSession(data);
            CheckSessionState(data);
            
            var timings = new List<TimingModel>();

            foreach (var driver in _drivers)
            {
                if (!driver.IsPaceCar &&
                    driver.IsSpectator == 0 &&
                    data.Telemetry.CarIdxPosition[driver.CarIdx] > 0)
                {
                    ProcessLapChange(data, driver.CarIdx);
                    ProcessPitlane(data, driver.CarIdx);

                    timings.Add(new TimingModel
                    {
                        Name = driver.TeamName,
                        DriverName = driver.UserName,
                        CarNum = driver.CarNumber,
                        Position = data.Telemetry.CarIdxPosition[driver.CarIdx],
                        ClassPosition = data.Telemetry.CarIdxClassPosition[driver.CarIdx],
                        OnPitRoad = data.Telemetry.CarIdxOnPitRoad[driver.CarIdx],
                        ClassColor = "#" + driver.CarClassColor,
                        IRating = driver.IRating,
                        LicString = driver.LicString,
                        LicColor = driver.LicColor,
                        EstTime = data.Telemetry.CarIdxEstTime[driver.CarIdx],
                        PitTime = 0.0f,
                        PitLastTime = 0.0f,
                        PittedLap = 0.0f,
                        CarLap = data.Telemetry.CarIdxLap[driver.CarIdx],
                        StintLength = 0f,
                        LastLap = _laptimes[driver.CarIdx].LastOrDefault(),
                        TrackSurf = data.Telemetry.CarIdxTrackSurface[driver.CarIdx],
                        Gap = 0.0f,
                        DistDegree = data.Telemetry.CarIdxLapDistPct[driver.CarIdx] * 100,

                    });
                }
            }

            Timings = timings.OrderBy(x=>x.Position).ToList();

            AfterUpdate?.Invoke();
        }

        public void UpdateSession(DataSample data)
        {
            if (_drivers == null || _drivers != data.SessionData.DriverInfo.Drivers)
            {
                _drivers = data.SessionData.DriverInfo.Drivers;
            }

            if (_sectors == null || _sectors != data.SessionData.SplitTimeInfo.Sectors)
            {
                _sectors = data.SessionData.SplitTimeInfo.Sectors;
            }
        }

        private void ProcessPitlane(DataSample data, in long driverCarIdx)
        {
            
        }

        private void ProcessLapChange(DataSample data, in long driverCarIdx)
        {
            if (!_laptimes.ContainsKey(driverCarIdx))
            {
                _laptimes[driverCarIdx] = new List<double>();
                _currentLap[driverCarIdx] = 0;
            }

            if (data.Telemetry.CarIdxLap[driverCarIdx] == -1)
            {
                return;
            }

            if (_currentLap[driverCarIdx] != data.Telemetry.CarIdxLap[driverCarIdx])
            {
                var sessionTime = data.Telemetry.SessionTime;
                
                if (!_currentLapStartTime.ContainsKey(driverCarIdx))
                {
                    _currentLapStartTime[driverCarIdx] = sessionTime;
                    _currentLap[driverCarIdx] = data.Telemetry.CarIdxLap[driverCarIdx];
                    return;
                }

                var startTime = _currentLapStartTime[driverCarIdx];

                var lapTimeSeconds = (sessionTime - startTime);
                
                _laptimes[driverCarIdx].Add(lapTimeSeconds);
                _currentLapStartTime[driverCarIdx] = sessionTime;
                _currentLap[driverCarIdx] = data.Telemetry.CarIdxLap[driverCarIdx];

                var position = data.Telemetry.CarIdxPosition[driverCarIdx];
                if (position == 1)
                {
                    _gapInFront[driverCarIdx] = "---";
                }
                else
                {
                    for (int p = 0; p < data.Telemetry.CarIdxPosition.Length; p++)
                    {
                        if (data.Telemetry.CarIdxPosition[p] == position - 1)
                        {
                            if (data.Telemetry.CarIdxLap[driverCarIdx] == data.Telemetry.CarIdxLap[p])
                            {
                                _gapInFront[driverCarIdx] =
                                    (_currentLapStartTime[driverCarIdx] - _currentLapStartTime[p]).Seconds().ToString(@"mm\:ss\.fff");
                            }
                            else
                            {
                                _gapInFront[driverCarIdx] =
                                    $"{(data.Telemetry.CarIdxLap[p] - data.Telemetry.CarIdxLap[driverCarIdx])} L";
                            }
                            break;
                        }
                    }
                }
            }

        }

        private void CheckSessionState(DataSample data)
        {
            if (_currentStateNumber != data.Telemetry.SessionNum)
            {
                _laptimes = new ConcurrentDictionary<long, List<double>>();
                _stindRecord = new ConcurrentDictionary<long, List<double>>();
                _currentLap = new ConcurrentDictionary<long, int>();
                _currentLapStartTime = new ConcurrentDictionary<long, double>();
                _gapInFront = new ConcurrentDictionary<long, string>();

                _currentStateNumber = data.Telemetry.SessionNum;
            }
        }

        public event Action AfterUpdate;
    }

    public class TimingModel
    {
        public string Name { get; set; }
        public string DriverName { get; set; }
        public string CarNum { get; set; }
        public int Position { get; set; }
        public int ClassPosition { get; set; }
        public bool OnPitRoad { get; set; }
        public float DistDegree { get; set; }
        public double Gap { get; set; }
        public TrackLocation TrackSurf { get; set; }
        public double LastLap { get; set; }
        public string ClassColor { get; set; }
        public long IRating { get; set; }
        public string LicString { get; set; }
        public string LicColor { get; set; }
        public double EstTime { get; set; }
        public double PitTime { get; set; }
        public double PitLastTime { get; set; }
        public float PittedLap { get; set; }
        public int CarLap { get; set; }
        public float StintLength { get; set; }
        public bool Selected { get; set; }
        public bool IsFinished { get; set; }
    }
}
