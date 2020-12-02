using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class DriverInfoBase : IRacingComponentBase
    {
        [Parameter]
        public bool Photo { get; set; }
        [Parameter]
        public bool Laptimes { get; set; }


        public DriverInfoBase() : base("driver-details")
        {
            ClassMapper
                .If("photo", () => Photo)
                .If("lap-times", () => Laptimes);

        }

    }
}
