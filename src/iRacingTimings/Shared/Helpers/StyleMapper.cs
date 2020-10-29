using System.Linq;

namespace iRacingTimings.Shared
{
    public class StyleMapper : BaseMapper
    {
        public string AsString()
        {
            return string.Join("; ", Items.Select(i => i()).Where(i => i != null));
        }


        public override string ToString()
        {
            return AsString();
        }
    }
}
