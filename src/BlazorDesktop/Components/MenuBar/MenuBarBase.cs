using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorDesktop.Components
{
    public class MenuBarBase : BaseComponent, IMenuBarSubMenuToggler
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public MenuBarButtonBase CurrentButton { get; private set; }
        public MenuBarSubMenuBase CurrentSubmenu { get; private set; }

        public Task ToggleSubMenuAsync()
        {
            throw new NotImplementedException();
        }

        public async Task ToggleSelectedAsync(MenuBarButtonBase menuButton, MenuBarSubMenuBase subMenu)
        {
            if (menuButton.Selected)
            {
                var currentButton = CurrentButton;
                CurrentButton = menuButton;

                if (CurrentButton != null && currentButton != menuButton && currentButton.Selected)
                {
                    await currentButton.ToggleSelectedAsync();
                }
            }

            if (subMenu != null)
            {
                await subMenu.ToggleSelectedAsync();
                CurrentSubmenu = subMenu;
            }
            else
            {
                CurrentSubmenu = null;
            }
        }
    }
}