using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using iRacingSDK;
using iRacingTimings.Data;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class StandingsBase : IRacingComponentBase
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

        public StandingsBase() : base("standings")
        {
        }

        public override void UpdateSession(object sender, DataSample data)
        {
            if (_drivers == null || _drivers != data.SessionData.DriverInfo.Drivers)
            {
                _drivers = data.SessionData.DriverInfo.Drivers;
            }

            if (_sectors == null || _sectors != data.SessionData.SplitTimeInfo.Sectors)
            {
                _sectors = data.SessionData.SplitTimeInfo.Sectors;
            }

            StateHasChanged();
        }

        
        public override void Update(object sender, DataSample data)
        {

            UpdateSession(sender, data);
            CheckSessionState(data);

            var timings = new List<TimingModel>();

            var camPos = data.Telemetry.Positions[data.Telemetry.CamCarIdx];
            var offset = camPos - 25 > 0 ? camPos - 25 : 0;

            foreach (var driver in _drivers)
            {
                if (!driver.IsPaceCar &&
                    driver.IsSpectator == 0 &&
                    data.Telemetry.CarIdxPosition[driver.CarIdx] > 0)
                {
                    ProcessLapChange(data, driver.CarIdx);
                    ProcessPitlane(data, driver.CarIdx);

                    byte[] bytes = Encoding.Default.GetBytes(driver.UserName);
                    var username = Encoding.UTF8.GetString(bytes);

                    timings.Add(new TimingModel
                    {
                        Name = driver.TeamName,
                        DriverName = username,
                        CarNum = driver.CarNumber,
                        Position = data.Telemetry.CarIdxPosition[driver.CarIdx],
                        ClassPosition = data.Telemetry.CarIdxClassPosition[driver.CarIdx],
                        OnPitRoad = data.Telemetry.CarIdxOnPitRoad[driver.CarIdx],
                        ClassColor =  "#" + driver.CarClassColor.Substring(2),
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
                        Selected = data.Telemetry.CamCarIdx == driver.CarIdx,
                        IsFinished = data.Telemetry.HasData((int)driver.CarIdx) && data.Telemetry.HasSeenCheckeredFlag[driver.CarIdx]
                    });
                }
            }

            Timings = timings.OrderBy(x => x.Position).ToList();


            StateHasChanged();
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
    }
}
