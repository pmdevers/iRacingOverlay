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
        public TimingModel Driver { get; set; }

        [Parameter]
        public bool ShowData { get; set; }
        public bool ShowLastLaptime { get; set; }

        public DriverBase()
        {
            ClassMapper.Add("driver-wrapper")
                .If("position-displays-selected-driver", () => Driver.Selected)
                .If("allow-driver", () => true)
                .If("show-driver", () => true)
                .If("show-pit", () => Driver.OnPitRoad)
                .If("show-lap-time", () => !Driver.OnPitRoad && Driver.DistDegree < 10 && Driver.DistDegree > 0)
                .If("show-flag-7x", () => Driver.IsFinished)
                .If("odd", () => Driver.Position % 2 > 0)
                .If("even", () => Driver.Position % 2 == 0);

            DataClassMapper.Add("data-wrapper")
                .If("allow-data", () => ShowData)
                .If("show-last-lap-time", () => ShowLastLaptime);

            StyleMapper.GetIf(() => $"transform: TranslateY({Driver.Position * 28}px);", () => Driver != null);
        }
    }
}
