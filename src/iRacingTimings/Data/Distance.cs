using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace iRacingTimings.Data
{
    public enum Scale
    {
        Unknown,
        Millimetre,
        Centimetre,
        Decimetre,
        Metre,
        Decametre,
        Hectometre,
        Kilometres
    }

    public readonly struct Distance
    {
        private readonly double _value;

        public Distance(double meters)
        {
            _value = meters;
            Scale = Scale.Metre;
        }

        public Scale Scale { get; }

        public static bool TryParse(string s, IFormatProvider formatProvider, out Distance result)
        {
            result = default;
            if (!string.IsNullOrEmpty(s))
            {
                var marker = GetMarkerType(s);
                s = RemoveMarks(s);
                if (marker != Scale.Unknown && double.TryParse(s, NumberStyles.Number, formatProvider, out var dec))
                {
                    dec *= Dividers[marker];
                    result = Create(dec);
                    return true;
                }
            }

            return false;
        }

        public static Distance Create(double meters)
        {
            return new Distance(meters);
        }

        public static explicit operator string(Distance val)
        {
            return val.ToString(CultureInfo.InvariantCulture);
        }

        public static explicit operator Distance(string str)
        {
            return TryParse(str, CultureInfo.InvariantCulture, out var dis) ? dis : default;
        }

        public static implicit operator Distance(double val)
        {
            return new Distance(val);
        }

        public static explicit operator double(Distance val)
        {
            return val._value;
        }

        public static double operator /(double d, Distance p) => d / p._value;
        public static int operator /(int d, Distance p) => (int)Math.Round(d / p._value);
        public static double operator /(Distance p, double d) => p._value / d;
        public static int operator /(Distance p, int d) => (int)Math.Round(p._value / d);

        public static double operator *(double d, Distance p) => d * p._value;
        public static int operator *(int d, Distance p) => (int)Math.Round(d * p._value);
        public static double operator *(Distance p, double d) => p._value * d;
        public static int operator *(Distance p, int d) => (int)Math.Round(p._value * d);

        public static bool operator >(double d, Distance p) => d > p._value;
        public static bool operator <(double d, Distance p) => d < p._value;
        public static bool operator >(Distance p, double d) => p._value > d;
        public static bool operator <(Distance p, double d) => p._value < d;
        public static bool operator >(Distance p, Distance d) => p._value > d._value;
        public static bool operator <(Distance p, Distance d) => p._value < d._value;

        public static bool operator >=(double d, Distance p) => d > p._value;
        public static bool operator <=(double d, Distance p) => d < p._value;
        public static bool operator >=(Distance p, double d) => p._value > d;
        public static bool operator <=(Distance p, double d) => p._value < d;
        public static bool operator >=(Distance p, Distance d) => p._value > d._value;
        public static bool operator <=(Distance p, Distance d) => p._value < d._value;


        public string ToString(string format, IFormatProvider formatProvider)
        {
            var marker = GetMarkerType(format ?? string.Empty);
            if (marker == Scale.Unknown) throw new FormatException();

            var doubleValue = _value / Dividers[marker];
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
            return ToString("0.#################### m", provider);
        }

        internal static readonly Dictionary<Scale, double> Dividers = new Dictionary<Scale, double>
        {
            {Scale.Millimetre, 0.001d},
            {Scale.Centimetre, 0.01d},
            {Scale.Decimetre, 0.1d},
            {Scale.Metre, 1d},
            {Scale.Decametre, 10d},
            {Scale.Hectometre, 100d},
            {Scale.Kilometres, 1000d}
        };

        internal static readonly Dictionary<string, Scale> MarkerTypes = new Dictionary<string, Scale>
        {
            {"mm", Scale.Millimetre},
            {"dm", Scale.Decimetre},
            {"cm", Scale.Centimetre},
            {"m", Scale.Metre},
            {"dam", Scale.Decametre},
            {"hm", Scale.Hectometre},
            {"km", Scale.Kilometres}
        };

        internal static readonly Dictionary<Scale, string> TypeMarkers =
            MarkerTypes.ToDictionary(x => x.Value, y => y.Key);


        private static Scale GetMarkerType(string str)
        {
            foreach (var marker in MarkerTypes.Keys.OrderByDescending(x => x.Length))
                if (str.EndsWith(marker, StringComparison.Ordinal))
                    return MarkerTypes[marker];

            return Scale.Unknown;
        }

        public static string RemoveMarks(string str)
        {
            foreach (var mark in MarkerTypes.Keys.OrderByDescending(x => x.Length))
                str = str.Replace(mark, string.Empty);

            return str;
        }

        public static Distance Parse(string distance)
        {
            if (TryParse(distance, CultureInfo.InvariantCulture, out var result)) return result;

            return default;
        }
    }
}