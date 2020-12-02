using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class DriverBase : BaseDomComponent
    {
        public ClassMapper DataClassMapper { get; } = new ClassMapper();

        
        [Parameter]
        public bool ShowData { get; set; }

        [Parameter]
        public iRacingSDK.Driver Item { get; set; }

        public DriverBase()
        {
            ClassMapper.Add("driver-wrapper")
                .If("position-displays-selected-driver", () => Item?.IsCurrentDriver ?? false)
                .If("allow-driver", () => Item != null && Item.Live.Position > 0)
                .If("show-driver", () => Item != null && !Item.IsPaceCar && !Item.IsSpectator && Item.Live.Position > 0)
                ////.If("show-pit", () => Driver.OnPitRoad)
                //.If("show-lap-time", () => !Item.OnPitRoad && Driver.DistDegree < 10 && Driver.DistDegree > 0)
                ////.If("show-flag-7x", () => Driver.IsFinished)
                .If("odd", () => Item != null && Item.Live.Position % 2 > 0)
                .If("even", () => Item != null && Item.Live.Position % 2 == 0);

            //DataClassMapper.Add("data-wrapper")
            //    .If("allow-data", () => ShowData)
            //    .If("show-last-lap-time", () => ShowLastLaptime);

            StyleMapper.GetIf(() => $"transform: TranslateY({ Item.Live.Position * 28}px);", () => Item != null);
        }
    }
}
