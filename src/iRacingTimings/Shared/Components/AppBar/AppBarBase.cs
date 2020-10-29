using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace iRacingTimings.Shared.Components
{
	public class AppBarBase : BaseDomComponent
	{
        [Parameter]
        public RenderFragment ChildContent {get; set; }

        [Parameter]
        public bool MaximizeBox { get; set; } = true;

        [Parameter]
        public bool MinimizeBox { get; set; } = true;

        [Parameter]
        public string AppIcon { get; set; }

        [Parameter]
        public bool IsMaximizable { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnMinimize { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnMaximize { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnClose { get; set; }

        public AppBarBase()
        {
            
        }

        protected async Task Maximize(MouseEventArgs e)
        {
            IsMaximizable = !IsMaximizable;
            await OnMaximize.InvokeAsync(e);
            StateHasChanged();
        }

        public string GetIcon()
        {
            return new StyleMapper()
                .If($"background-image: url('{AppIcon}')", () => !string.IsNullOrEmpty(AppIcon))
                .AsString();
        }
    }
}
