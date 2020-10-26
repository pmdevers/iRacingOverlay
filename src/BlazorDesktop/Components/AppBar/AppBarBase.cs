using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorDesktop.Components
{
	public class AppBarBase : BaseComponent
	{
        [Parameter]
        public RenderFragment ChildContent {get; set; }

        [Parameter]
        public bool MaximizeBox { get; set; } = true;

        [Parameter]
        public bool MinimizeBox { get; set; } = true;

        [Parameter]
        public EventCallback<MouseEventArgs> OnMinimize { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnMaximize { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnClose { get; set; }
    }
}
