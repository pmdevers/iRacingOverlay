using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components
{
    public class MenuBarButtonTitleBase : BaseComponent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
