using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingSDK;
using iRacingTimings.Data;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class DriverInfoBase : IRacingComponentBase
    {
        [Parameter]
        public bool Photo { get; set; }
        [Parameter]
        public bool Laptimes { get; set; }

        public int Position { get; set; }
        public string Color { get; set; }
        public string FirstName { get; set; }
        public int Gain { get; set; }
        public string Number { get; set; }
        public string NumberDesign { get; set; }
        public string Suffix { get; set; }
        public string LastName { get; set; }
        public string FlagImage { get; set; }


        public Car Car { get; set; }

        public DriverInfoBase() : base("driver-details")
        {
            ClassMapper
                .If("photo", () => Photo)
                .If("lap-times", () => Laptimes);

        }

        public override void UpdateSession(object sender, DataSample data)
        {

        }

        public override void Update(object sender, DataSample data)
        {
            Car = new Car(data.Telemetry, data.Telemetry.CamCarIdx);

            Position = Car.OfficialPostion;
            Color = Car.Details.Color;
            FirstName = Car.Details.FirstName;
            LastName = Car.Details.LastName;
            Suffix = Car.Details.Suffix;
            
            FlagImage = $"./img/clubs/{Car.Details.ClubName}.svg";
            //Gain = 

            StateHasChanged();
        }

        
    }
}
