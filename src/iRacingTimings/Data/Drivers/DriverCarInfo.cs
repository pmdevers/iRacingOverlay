using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;

namespace iRacingTimings.Data.Drivers
{
    public class DriverCarInfo
    {
        private readonly Driver _driver;

        public DriverCarInfo(Driver driver)
        {
            _driver = driver;
        }

        public int Id { get; private set; }
        public string Number { get; private set; }
        public int NumberRaw { get; private set; }
        public string Name { get; private set; }
        public int ClassId { get; private set; }
        public int ClassRelSpeed { get; private set; }
        public string ClassColor { get; private set; }
        public string ClassShortName { get; private set; }
        public string ShortName { get; private set; }
        public string Path { get; private set; }

        public void Update(SessionData sessionData)
        {
            var info = sessionData.DriverInfo.Drivers[_driver.Id];
            Id = info.CarID;
            Number = info.CarNumber;
            NumberRaw = info.CarNumberRaw;
            Name = info.CarScreenName;
            ClassId = info.CarClassID;
            ClassRelSpeed = info.CarClassRelSpeed;
            ClassColor = info.CarClassColor.Replace("0x" , "#");
            ClassShortName = info.CarClassShortName;
            ShortName = info.CarScreenNameShort;
            Path = info.CarPath;
        }
    }
}
