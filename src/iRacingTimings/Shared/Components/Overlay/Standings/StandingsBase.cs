using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using iRacingSDK;
using iRacingTimings.Data;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class StandingsBase : IRacingComponentBase
    {
        
        public StandingsBase() : base("standings")
        {
        }

        //protected override void OnUpdate(object? sender, Telemetry e)
        //{
        //    StateHasChanged();
        //}
    }
}
