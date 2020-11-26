using iRacingSDK;
using iRacingTimings.Data;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public abstract class IRacingComponentBase : BaseDomComponent
    {
        [Parameter]
        public bool Visible { get; set; }

        public Simulator Simulator => Simulator.Instance;

        protected IRacingComponentBase(string name)
        {
            ClassMapper.Add(name)
                .If("hide", () => !Visible);

            
            Simulator.TelemetryUpdated += OnUpdate;
            
            if (!Simulator.iRacing.IsListening)
            {
                Simulator.Start();
            }
        }

        protected virtual void OnUpdate(object sender, Telemetry e) => InvokeAsync(() => StateHasChanged());
    }
}
