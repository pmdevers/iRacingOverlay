using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class AdminMessageBase : BaseDomComponent
    {
        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public bool Visible { get; set; }

        public AdminMessageBase()
        {
            ClassMapper.Add("admin-message")
                .If("hide", () => !Visible);
        }
    }
}
