using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;
using iRacingTimings.Data;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class DriverBase : BaseDomComponent
    {
        public ClassMapper DataClassMapper { get; } = new ClassMapper();

        [Parameter]
        public Data.Drivers.Driver Driver { get; set; }

        [Parameter]
        public bool ShowData { get; set; }
        [Parameter]
        public bool ShowLastLaptime { get; set; }
        [Parameter]
        public bool ShowLicense { get; set; }

        public DriverBase()
        {
            ClassMapper.Add("driver-wrapper")
                .If("position-displays-selected-driver", () => Driver.IsCurrentDriver)
                .If("allow-driver", () => true)
                .If("show-driver", () => true)
                .If("show-pit", () => Driver.Pit.InPitLane)
                .If("show-lap-time", () => !Driver.Pit.InPitLane && Driver.Live.LapDistance < 10 && Driver.Live.LapDistance > 0)
                //.If("show-flag-7x", () => Driver.IsFinished)
                .If("odd", () => Driver.Live.Position % 2 > 0)
                .If("even", () => Driver.Live.Position % 2 == 0);

            DataClassMapper.Add("data-wrapper")
                .If("allow-data", () => ShowData)
                .If("show-last-lap-time", () => ShowLastLaptime)
                .If("show-license", () => ShowLicense);

            StyleMapper.GetIf(() => $"transform: TranslateY({Driver.Live.Position * 28}px);", () => Driver != null);
        }
    }
}
