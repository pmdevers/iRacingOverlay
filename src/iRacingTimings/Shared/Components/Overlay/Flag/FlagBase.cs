using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRacingTimings.Shared.Components;
using Microsoft.AspNetCore.Components;

namespace iRacingTimings.Shared.Components.Overlay {

    public class FlagBase : BaseDomComponent
    {
        public ClassMapper InnerClassMapper = new ClassMapper();

        [Parameter]
        public FlagScale Scale { get; set; }

        public FlagBase()
        {
            ClassMapper
                .If("flag-wrapper-9x5", () => Scale == FlagScale.flag9x5)
                .If("flag-wrapper-7x5", () => Scale == FlagScale.flag7x5);

            InnerClassMapper
                .If("flag-9x5", () => Scale == FlagScale.flag9x5)
                .If("flag-7x5", () => Scale == FlagScale.flag7x5);
        }

        protected override void OnParametersSet()
        {
           

            base.OnParametersSet();
        }
    }


    public enum FlagScale
    {
        flag9x5,
        flag7x5
    }
}
