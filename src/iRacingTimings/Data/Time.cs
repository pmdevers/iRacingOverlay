using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingTimings.Data
{
    public struct Time
    {
        private double _value;
        
        public Time(double value)
        {
            _value = value;
        }

        public static Time Create(double seconds) => new Time(seconds);

        public static implicit operator Time(double val) => new Time(val);

        public static explicit operator double(Time val) => val._value;

        public static double operator +(double d, Time p) => d + p._value;
        public static double operator +(Time p, double d) => p._value + d;
        public static Time operator +(Time p, Time d) => p._value + d._value;

        public static double operator -(double d, Time p) => d - p._value;
        public static double operator -(Time p, double d) => p._value - d;
        public static Time operator -(Time p, Time d) => p._value - d._value;

        public static double operator *(double d, Time p) => d * p._value;
        public static double operator *(Time p, double d) => p._value * d;
        public static Time operator *(Time p, Time d) => p._value * d._value;

        public static double operator /(double d, Time p) => d / p._value;
        public static double operator /(Time p, double d) => p._value / d;
        public static Time operator /(Time p, Time d) => p._value / d._value;

        public static bool operator >(double d, Time p) => d > p._value;
        public static bool operator <(double d, Time p) => d < p._value;
        public static bool operator >(Time p, double d) => p._value > d;
        public static bool operator <(Time p, double d) => p._value < d;
        public static bool operator >(Time p, Time d) => p._value > d._value;
        public static bool operator <(Time p, Time d) => p._value < d._value;

        public static bool operator >=(double d, Time p) => d > p._value;
        public static bool operator <=(double d, Time p) => d < p._value;
        public static bool operator >=(Time p, double d) => p._value > d;
        public static bool operator <=(Time p, double d) => p._value < d;
        public static bool operator >=(Time p, Time d) => p._value > d._value;
        public static bool operator <=(Time p, Time d) => p._value < d._value;

        public override string ToString()
        {
            var ts = TimeSpan.FromSeconds(_value);
            
            var sb = new StringBuilder();
            var hours = (int)ts.TotalHours;

            if (hours > 0)
            {
                sb.Append(hours.ToString().PadLeft(2, '0'));
                sb.Append(":");
            }

            sb.Append(ts.Minutes.ToString().PadLeft(2, '0'));
            sb.Append(":");
            sb.Append(ts.Seconds.ToString().PadLeft(2, '0'));

            return sb.ToString();
        }
    }
}
