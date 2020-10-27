using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace BlazorDesktop.Components
{
    public class MenuBarButtonTitleBase : BaseComponent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
