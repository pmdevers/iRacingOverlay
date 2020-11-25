using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class ColorStripeBase : BaseDomComponent
    {
        [Parameter]
        public string Color { get; set; }

        public ColorStripeBase()
        {
            ClassMapper.Add("color-stripe");
        }

        protected override void OnParametersSet()
        {
            StyleMapper = new StyleMapper();
            StyleMapper.If($"background-color: {Color};", () => !string.IsNullOrEmpty(Color));
            base.OnParametersSet();
        }
    }
}
