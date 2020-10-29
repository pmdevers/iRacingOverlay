using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components
{
    public class MenuBarSubMenuBase : BaseComponent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter]
        public MenuBarButtonBase Button { get; set; }
    }
}
