using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;

namespace iRacingTimings.Data.Drivers
{
    public class DriverQualyResults
    {
        private readonly Driver _driver;

        public int Position { get; set; }
        public int ClassPosition { get; set; }
        public LapTime Lap { get; set; }

        public DriverQualyResults(Driver driver)
        {
            _driver = driver;
        }

        public void Update(SessionData info)
        {
            

            //Position = results.ResultsPositions[_driver.Id];
            //ClassPosition = results.
        }
    }
}
