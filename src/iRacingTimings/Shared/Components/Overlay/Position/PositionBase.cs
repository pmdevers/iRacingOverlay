using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class PositionBase : BaseDomComponent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public PositionBase()
        {
            
        }
    }
}
