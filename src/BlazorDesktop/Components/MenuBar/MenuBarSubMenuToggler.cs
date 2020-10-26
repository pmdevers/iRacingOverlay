using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDesktop.Components
{
    public interface IMenuBarSubMenuToggler
    {
        Task ToggleSubMenuAsync();
    }
}
