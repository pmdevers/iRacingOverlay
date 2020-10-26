using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorDesktop.Components
{
    public class MenuBarButtonBase : BaseComponent
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter]
        public MenuBarBase MenuBar { get; set; }

        [CascadingParameter]
        public MenuBarSubMenuBase SubMenu { get; set; }

        [Parameter]
        public ICommand Command { get; set; }

        [Parameter]
        public object CommandParameter { get; set; }

        [Parameter]
        public bool Selected { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool AllowSelection { get; set; }

        [Parameter] 
        public EventCallback<bool> SelectedChanged { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        public async Task ToggleSelectedAsync()
        {
            this.Selected = !this.Selected;

            await this.SelectedChanged.InvokeAsync(this.Selected);

            if (MenuBar != null)
            {
                await this.MenuBar.ToggleSelectedAsync(this, SubMenu);
            }

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
    }
}
