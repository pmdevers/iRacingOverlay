using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace iRacingTimings.Data
{
    public enum SpeedScale
    {
        Unknown,
        Mph,
        Kph
    }

    public readonly struct Speed
    {
        private readonly double _value;

        public Speed(double mph)
        {
            _value = mph;
            SpeedScale = SpeedScale.Mph;
        }

        public SpeedScale SpeedScale { get; }

        public static bool TryParse(string s, IFormatProvider formatProvider, out Speed result)
        {
            result = default;
            if (!string.IsNullOrEmpty(s))
            {
                var marker = GetMarkerType(s);
                s = RemoveMarks(s);
                if (marker != SpeedScale.Unknown && double.TryParse(s, NumberStyles.Number, formatProvider, out var dec))
                {
                    dec *= Dividers[marker];
                    result = Create(dec);
                    return true;
                }
            }

            return false;
        }

        public static Speed Create(double meters)
        {
            return new Speed(meters);
        }

        public static explicit operator string(Speed val)
        {
            return val.ToString(CultureInfo.InvariantCulture);
        }

        public static explicit operator Speed(string str)
        {
            return TryParse(str, CultureInfo.InvariantCulture, out var dis) ? dis : default;
        }

        public static implicit operator Speed(double val)
        {
            return new Speed(val);
        }

        public static explicit operator double(Speed val)
        {
            return val._value;
        }

        public static double operator /(double d, Speed p) => d / p._value;
        public static int operator /(int d, Speed p) => (int)Math.Round(d / p._value);
        public static double operator /(Speed p, double d) => p._value / d;
        public static int operator /(Speed p, int d) => (int)Math.Round(p._value / d);

        public static double operator *(double d, Speed p) => d * p._value;
        public static int operator *(int d, Speed p) => (int)Math.Round(d * p._value);
        public static double operator *(Speed p, double d) => p._value * d;
        public static int operator *(Speed p, int d) => (int)Math.Round(p._value * d);

        public static bool operator >(double d, Speed p) => d > p._value;
        public static bool operator <(double d, Speed p) => d < p._value;
        public static bool operator >(Speed p, double d) => p._value > d;
        public static bool operator <(Speed p, double d) => p._value < d;
        public static bool operator >(Speed p, Speed d) => p._value > d._value;
        public static bool operator <(Speed p, Speed d) => p._value < d._value;

        public static bool operator >=(double d, Speed p) => d > p._value;
        public static bool operator <=(double d, Speed p) => d < p._value;
        public static bool operator >=(Speed p, double d) => p._value > d;
        public static bool operator <=(Speed p, double d) => p._value < d;
        public static bool operator >=(Speed p, Speed d) => p._value > d._value;
        public static bool operator <=(Speed p, Speed d) => p._value < d._value;


        public string ToString(string format, IFormatProvider formatProvider)
        {
            var marker = GetMarkerType(format ?? string.Empty);
            if (marker == SpeedScale.Unknown) throw new FormatException();

            var doubleValue = _value * Dividers[marker];
            var str = doubleValue.ToString(RemoveMarks(format ?? string.Empty), formatProvider);

            return $"{str}{TypeMarkers[marker]}";
        }

        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(IFormatProvider provider)
        {
            return ToString("0.#################### Mph", provider);
        }

        internal static readonly Dictionary<SpeedScale, double> Dividers = new Dictionary<SpeedScale, double>
        {
            {SpeedScale.Mph, 0},
            {SpeedScale.Kph, 3.6}
        };

        internal static readonly Dictionary<string, SpeedScale> MarkerTypes = new Dictionary<string, SpeedScale>
        {
            {"mph", SpeedScale.Mph},
            {"km/h", SpeedScale.Kph},
        };

        internal static readonly Dictionary<SpeedScale, string> TypeMarkers =
            MarkerTypes.ToDictionary(x => x.Value, y => y.Key);


        private static SpeedScale GetMarkerType(string str)
        {
            foreach (var marker in MarkerTypes.Keys.OrderByDescending(x => x.Length))
                if (str.EndsWith(marker, StringComparison.Ordinal))
                    return MarkerTypes[marker];

            return SpeedScale.Unknown;
        }

        public static string RemoveMarks(string str)
        {
            foreach (var mark in MarkerTypes.Keys.OrderByDescending(x => x.Length))
                str = str.Replace(mark, string.Empty);

            return str;
        }

        public static Speed Parse(string Speed)
        {
            if (TryParse(Speed, CultureInfo.InvariantCulture, out var result)) return result;

            return default;
        }
    }
}