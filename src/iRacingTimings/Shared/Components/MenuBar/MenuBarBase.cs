using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components
{
    public class MenuBarBase : BaseDomComponent, IMenuBarSubMenuToggler
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public MenuBarButtonBase CurrentButton { get; private set; }
        
        public Task ToggleSubMenuAsync()
        {
            throw new NotImplementedException();
        }

        public MenuBarBase()
        {
            ClassMapper.Add("menu-bar");
        }

        public async Task ToggleSelectedAsync(MenuBarButtonBase menuButton)
        {
            if (menuButton.Selected)
            {
                var currentButton = CurrentButton;
                CurrentButton = menuButton;

                if (currentButton != null && currentButton != menuButton && currentButton.Selected)
                {
                    await currentButton.ToggleSelectedAsync();
                }
            }
        }
    }
}