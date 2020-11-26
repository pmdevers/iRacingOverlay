using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;

namespace iRacingTimings.Data.Drivers
{
    public class DriverTeamInfo
    {
        private readonly Driver _driver;

        public int Id { get; private set; }
        public string Name { get; private set; }

        public DriverTeamInfo(Driver driver)
        {
            _driver = driver;
        }

        public void Update(SessionData sessionData)
        {
            var info = sessionData.DriverInfo.Drivers[_driver.Id];

            Id = info.TeamID;
            Name = info.TeamName;
        }

    }
}
