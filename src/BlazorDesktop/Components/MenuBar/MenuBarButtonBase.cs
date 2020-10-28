using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorDesktop.Components
{
    public class MenuBarButtonBase : BaseDomComponent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        
        [CascadingParameter]
        public MenuBarBase MenuBar { get; set; }
        
        [Parameter]
        public ICommand Command { get; set; }

        [Parameter]
        public object CommandParameter { get; set; }

        [Parameter]
        public bool Selected { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter] 
        public bool AllowSelection { get; set; } = true;

        [Parameter] 
        public EventCallback<bool> SelectedChanged { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        public MenuBarButtonBase()
        {
            ClassMapper.Add("menu-bar-button")
                .If("open", () => Selected);
        }

        public async Task ToggleSelectedAsync()
        {
            this.Selected = !this.Selected;

            await this.SelectedChanged.InvokeAsync(this.Selected);

            await MenuBar.ToggleSelectedAsync(this);
            this.StateHasChanged();
        }

        protected async void OnClickHandler(MouseEventArgs args)
        {
            if (Disabled)
            {
                return;
            }

            if (AllowSelection)
            {
                await this.ToggleSelectedAsync();
            }

            await OnClick.InvokeAsync(args);
            if (Command?.CanExecute(CommandParameter) ?? false)
            {
                Command.Execute(CommandParameter);
            }
        }

        public string GetClass()
        {
            return Selected ? "open" : string.Empty;
        }
    }
}
