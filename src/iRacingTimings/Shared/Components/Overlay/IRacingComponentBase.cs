using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API.Entities;
using iRacingSDK;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public abstract class IRacingComponentBase : BaseDomComponent
    {
        private static readonly iRacingEvents _events = new iRacingEvents();

        [Parameter]
        public bool Visible { get; set; }

        protected IRacingComponentBase(string name)
        {
            ClassMapper.Add(name)
                .If("hide", () => !Visible);

            _events.NewData += Sdk_Update;
            _events.NewSessionData += Sdk_UpdateSession;
            _events.Connected += Connected;
            _events.Disconnected += Disconnected;

            if (!_events.IsListening)
            {
                _events.StartListening();
            }

            OnSessionUpdate += UpdateSession;
            OnUpdate += Update;
        }

        private void Disconnected()
        {
            StateHasChanged();
        }

        private void Connected()
        {
            StateHasChanged();
        }

        private void Sdk_Update(DataSample data)
        {
            OnUpdate?.Invoke(this, data);
        }

        private void Sdk_UpdateSession(DataSample data)
        {
            OnSessionUpdate?.Invoke(this, data);
        }

        public EventHandler<DataSample> OnSessionUpdate;
        public EventHandler<DataSample> OnUpdate;

        public abstract void UpdateSession(object sender, DataSample data);
        public abstract void Update(object sender, DataSample data);
    }
}
