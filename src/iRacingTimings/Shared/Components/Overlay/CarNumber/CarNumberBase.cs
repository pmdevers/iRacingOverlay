using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay
{
    public class CarNumberBase : BaseDomComponent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Design { get; set; }

        private string _backgroundColor;
        private string _forecolor;
        private string _strokeColor;

        public CarNumberBase()
        {
            ClassMapper.Add("number");
            StyleMapper.GetIf(() => $"background-color: #{_backgroundColor};", () => !string.IsNullOrEmpty(Design));
            StyleMapper.GetIf(() => $"color: #{_forecolor};", () => !string.IsNullOrEmpty(Design));
            StyleMapper.GetIf(() => $"-webkit-text-stroke: 1px #{_strokeColor}", () => !string.IsNullOrEmpty(Design));
        }

        protected override void OnParametersSet()
        {
            if (!string.IsNullOrEmpty(Design))
            {
                var element = Design.Split(",");
                _forecolor = element[2];
                _backgroundColor = element[3];
                _strokeColor = element[4];
            }

            base.OnParametersSet();
        }
    }
}
