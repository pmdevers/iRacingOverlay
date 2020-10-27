using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorDesktop.Components
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
        public bool IsMaximized { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnMinimize { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnMaximize { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnRestore { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClose { get; set; }

        public AppBarBase()
        {
            
        }

        public string GetIcon()
        {
            return new StyleMapper()
                .If($"background-image: url('{AppIcon}')", () => !string.IsNullOrEmpty(AppIcon))
                .AsString();
        }
    }
}
