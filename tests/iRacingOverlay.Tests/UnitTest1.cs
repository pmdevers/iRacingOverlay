using System;
using iRacingTimings.Data;
using Xunit;

namespace iRacingOverlay.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void DistanceTest()
        {
            var str = "3.65 km";

            var distance = Distance.Parse(str);

            var dis = new Distance(1000).ToString("0.## km");

            Assert.Equal(3650, distance);
        }

        [Fact]
        public void DivideByInt()
        {
            var str = "3.65 km";

            var distance = Distance.Parse(str);

            var result = distance / 10;

            
            Assert.Equal(365, result);
        }
    }
}
