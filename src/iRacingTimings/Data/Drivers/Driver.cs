using System.Dynamic;
using iRacingSDK;

namespace iRacingTimings.Data.Drivers
{
    public class Driver
    {
        private const string PACECAR_NAME = "safety pcfr500s";

        public Driver(int carIdx)
        {
            Id = carIdx;

            Team = new DriverTeamInfo(this);
            Car = new DriverCarInfo(this);
            Live = new DriverLiveInfo(this);
            Pit = new DriverPitInfo(this);
            Results = new DriverResults(this);
        }

        public int Id { get; }

        public string Name { get; private set; }
        public bool IsCurrentDriver { get; private set; }

        public int IRating { get; private set; }

        public string ShortName { get; private set; }

        public int CustId { get; private set; }

        public License License { get; private set; }

        public DriverTeamInfo Team { get; }
        public DriverCarInfo Car { get; }
        public DriverLiveInfo Live { get; }
        public DriverPitInfo Pit { get;  }
        public DriverResults Results { get; }
        public DriverSessionResults CurrentResults { get; private set; }

        public string DivisionName { get; private set; }

        public string ClubName { get; private set; }

        public int CarSponsor2 { get; private set; }

        public int CarSponsor1 { get; private set; }

        public string CarNumberDesign { get; private set; }

        public string CarDesign { get; private set; }

        public string HelmetDesign { get; private set; }

        public bool IsSpectator { get; private set; }
        public bool IsPacecar { get; private set; }


        public static Driver FromSessionData(SessionData sessionData, int carIdx)
        {
            var data = sessionData.DriverInfo.Drivers[carIdx];
            if (string.IsNullOrEmpty(data.UserName)) return null;

            var driver = new Driver(carIdx);

            driver.Update(sessionData);
            
            return driver;
        }


        public void Update(Telemetry telemetry)
        {
            Live.Update(telemetry);
            Pit.Update(telemetry);
            Results.Update(telemetry);
            CurrentResults?.Update(telemetry);
        }

        private void Update(SessionData sessionData)
        {
            var info = sessionData.DriverInfo.Drivers[Id];

            CurrentResults = new DriverSessionResults(this, Simulator.Instance.CurrentSession);

            Name = info.UserName;
            CustId = info.UserID;
            ShortName = info.AbbrevName;

            IRating = info.IRating;

            License = new License(info.LicLevel, info.LicSubLevel);

            IsSpectator = info.IsSpectator == 1;

            HelmetDesign = info.HelmetDesignStr;
            CarDesign = info.CarDesignStr;
            CarNumberDesign = info.CarNumberDesignStr;
            CarSponsor1 = info.CarSponsor_1;
            CarSponsor2 = info.CarSponsor_2;

            ClubName = info.ClubName;
            DivisionName = info.DivisionName;

            IsPacecar = CustId == -1 || Car.Name == PACECAR_NAME;
            
            Team.Update(sessionData);
            Car.Update(sessionData);
        }
    }
}