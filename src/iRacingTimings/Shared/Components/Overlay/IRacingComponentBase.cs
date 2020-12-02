using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API.Entities;
using iRacingSDK;
using iRacingSDK.Data;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public abstract class IRacingComponentBase : BaseDomComponent
    {
        [Parameter]
        public bool Visible { get; set; }

        protected IRacingComponentBase(string name)
        {
            ClassMapper.Add(name)
                .If("hide", () => !Visible);

            iRacing.OnTelemetry += IRacingOnOnTelemetry;
        }

        private void IRacingOnOnTelemetry(Telemetry obj)
        {
            InvokeAsync(StateHasChanged);
        }
    }
}
