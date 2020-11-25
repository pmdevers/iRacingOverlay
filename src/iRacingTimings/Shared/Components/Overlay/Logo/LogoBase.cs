using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class LogoBase : IRacingComponentBase
    {
        public LogoBase() : base("logo")
        {
            
        }


        public override void UpdateSession(object sender, DataSample data)
        {
            
        }

        public override void Update(object sender, DataSample data)
        {
            
        }
    }
}
