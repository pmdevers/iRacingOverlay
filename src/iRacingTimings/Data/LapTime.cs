using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingTimings.Data
{
    public struct LapTime
    {
        private double _value;
        
        public LapTime(double value)
        {
            _value = value;
        }

        public static LapTime Create(double seconds) => new LapTime(seconds);

        public static implicit operator LapTime(double val) => new LapTime(val);

        public static explicit operator double(LapTime val) => val._value;

        public static double operator +(double d, LapTime p) => d + p._value;
        public static double operator +(LapTime p, double d) => p._value + d;
        public static LapTime operator +(LapTime p, LapTime d) => p._value + d._value;

        public static double operator -(double d, LapTime p) => d - p._value;
        public static double operator -(LapTime p, double d) => p._value - d;
        public static LapTime operator -(LapTime p, LapTime d) => p._value - d._value;

        public static double operator *(double d, LapTime p) => d * p._value;
        public static double operator *(LapTime p, double d) => p._value * d;
        public static LapTime operator *(LapTime p, LapTime d) => p._value * d._value;

        public static double operator /(double d, LapTime p) => d / p._value;
        public static double operator /(LapTime p, double d) => p._value / d;
        public static LapTime operator /(LapTime p, LapTime d) => p._value / d._value;

        public static bool operator ==(double d, LapTime p) => d == p._value;
        public static bool operator !=(double d, LapTime p) => d != p._value;
        public static bool operator ==(LapTime p, double d) => p._value == d;
        public static bool operator !=(LapTime p, double d) => p._value != d;
        public static bool operator >(double d, LapTime p) => d > p._value;
        public static bool operator <(double d, LapTime p) => d < p._value;
        public static bool operator >(LapTime p, double d) => p._value > d;
        public static bool operator <(LapTime p, double d) => p._value < d;
        public static bool operator >(LapTime p, LapTime d) => p._value > d._value;
        public static bool operator <(LapTime p, LapTime d) => p._value < d._value;

        public override string ToString()
        {
            var ts = TimeSpan.FromSeconds(_value);
            return ts.ToString(@"m\:ss\.fff");
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is LapTime a)
            {
                return a._value == _value;
            }

            return false;
        }
    }
}
