using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    
    

    public class CarNumberBase : BaseDomComponent
    {
        [Parameter]
        public Car Car { get; set; }

        public CarNumberBase()
        {
            ClassMapper.Add("number");
            StyleMapper.GetIf(() => $"background-color: #{Car.Details.CarNumberDesign[3]};", () => Car != null);
            StyleMapper.GetIf(() => $"color: #{Car.Details.CarNumberDesign[2]};", () => Car != null);
            StyleMapper.GetIf(() => $"-webkit-text-stroke: 1px #{Car.Details.CarNumberDesign.Last()}", () => Car != null);
        }
    }
}
