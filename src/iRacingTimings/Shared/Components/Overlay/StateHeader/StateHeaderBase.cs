using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class StateHeaderBase : IRacingComponentBase
    {
        public StateHeaderBase() : base("state-header")
        {
            ClassMapper
                .If("show-checkered-flag", () => iRacing.LeaderHasFinished)
                .If("show-white-flag", () => iRacing.IsFinalLap)
                .If("show-yellow-flag", () => false)
                .If("show-full-course-yellow-flag", () => false);

        }
    }
}
