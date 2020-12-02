using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iRacingSDK.Data;

namespace iRacingSDK
{
    public class DriversCollection : IReadOnlyList<Driver>
    {
        private readonly List<Driver> _drivers = new List<Driver>();

        public IEnumerator<Driver> GetEnumerator()
        {
            return _drivers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _drivers.Count;
        public Driver this[int index] => _drivers[index];

        public Driver CurrentDriver { get; private set; }
        public Driver Leader { get; private set; }

        public IEnumerable<Driver> DriversOnly()
        {
            return _drivers.Where(x => !x.IsPaceCar);
        }

        internal DriversCollection() {}

        internal void Clear()
        {
            _drivers.Clear();
        }

        internal void Update(SessionData sessionData)
        {
            for (int id = 0; id < 70; id++)
            {
                var driver = _drivers.SingleOrDefault(x => x.Id == id);
                if (driver == null)
                {
                    driver = Driver.FromSession(sessionData, id);
                    if(driver == null) break;
                    _drivers.Add(driver);
                }
                else
                {
                    var oldId = driver.CustId;
                    var oldName = driver.Name;

                    driver.UpdateDynamicInfo(sessionData);
                    driver.Results.Update(sessionData);

                    if (oldId != driver.CustId)
                    {
                        // TODO RaiseEvent 
                    }
                }

                //if (driver.Id == iRacing.DriverId)
                //{
                //    CurrentDriver = driver;
                //    driver.IsCurrentDriver = true;
                //}
            }
            
        }

        internal void Update(Telemetry telemetry)
        {
            _drivers.ForEach(x =>
            {
                x.Live.Update(telemetry);
            });

            UpdatePositions(telemetry);
        }

        private void UpdatePositions(Telemetry telemetry)
        {
            if (telemetry.Session.SessionType == "Race" && telemetry.SessionState != SessionState.Checkered)
            {
                // Determine live position from lapdistance
                int pos = 1;
                foreach (var driver in _drivers.OrderByDescending(d => d.Live.TotalLapDistance))
                {
                    if (pos == 1) Leader = driver;
                    driver.Live.Position = pos;
                    pos++;
                }
            }
            else
            {
                foreach (var driver in _drivers.OrderBy(d => d.Results.Current.Position))
                {
                    if (this.Leader == null) Leader = driver;
                    driver.Live.Position = driver.Results.Current.Position;
                }
            }

            // Determine live class position from live positions and class
            // Group drivers in dictionary with key = classid and value = list of all drivers in that class
            var dict = (from driver in _drivers
                    group driver by driver.Car.CarClassId)
                .ToDictionary(d => d.Key, d => d.ToList());

            // Set class position
            foreach (var drivers in dict.Values)
            {
                var pos = 1;
                foreach (var driver in drivers.OrderBy(d => d.Live.Position))
                {
                    driver.Live.ClassPosition = pos;
                    pos++;
                }
            }

            //if (this.Leader != null && this.Leader.CurrentResults != null)
            //    .LeaderLap = this.Leader.CurrentResults.LapsComplete + 1;

        }
    }
}
