using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;

namespace iRacingTimings.Data
{
    public class Track
    {
        public int Id { get;}
        public string Name { get; private set; }
        public string CodeName { get; private set; }
        public Distance Length { get; private set; }
        public bool NightMode { get; private set; }
        public List<Sector> Sectors { get; }

        public Track(int trackId)
        {
            Id = trackId;
            Sectors = new List<Sector>();
        }

        public static Track FromSessionData(SessionData data)
        {
            var weekendInfo = data.WeekendInfo;
            var track = new Track(data.WeekendInfo.TrackID)
            {
                Name = weekendInfo.TrackDisplayName,
                CodeName = weekendInfo.TrackName,
                Length = (Distance) weekendInfo.TrackLength,
                NightMode = weekendInfo.WeekendOptions.NightMode == "1"
            };

            track.Sectors.Clear();

            foreach (var sector in data.SplitTimeInfo.Sectors)
            {
                track.Sectors.Add(new Sector()
                {
                    Number = sector.SectorNum,
                    StartPercentage = sector.SectorStartPct
                });
            }
            
            return track;
        }

        

        
    }

    public class Sector
    {
        public int Number { get; set; }
        public double StartPercentage { get; set; }

        public Time EnterSessionTime { get; set; }
        public LapTime SectorTime { get; set; }

        public Sector Copy()
        {
            var s = new Sector {Number = Number, StartPercentage = StartPercentage};
            return s;
        }
    }
}
