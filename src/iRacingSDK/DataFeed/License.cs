﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace iRacingSDK
{
    [Serializable]
    public class License
    {
        public License(int level, int sublevel)
        {
            SafetyRating = sublevel / 100f;
            Level = this.GetLevel(level);
            SortOrder = ((int)Level.Level) * 1000 + (int)sublevel;
        }

        public LicenseLevel Level { get; set; }
        public float SafetyRating { get; set; }
        public int SortOrder { get; set; }
        public LicenseLevel.Licenses LevelType => this.Level.Level;
        public string Name => this.Level.Name;
        
        public string Display
        {
            get
            {
                string level;
                if (this.Level.Level == LicenseLevel.Licenses.Unknown)
                    level = "?";
                else
                    level = this.Level.Display;

                var rating = SafetyRating.ToString("0.00", new CultureInfo("en-US"));

                return $"{level} {rating}";
            }
        }

        public override string ToString()
        {
            return this.Display;
        }

        private LicenseLevel GetLevel(int level)
        {
            return LicenseLevel.FromLevel(level);
        }

        [Serializable]
        public abstract class LicenseLevel
        {
            protected LicenseLevel(Licenses level, int lowRange, int highRange, string display = null)
            {
                this.Level = level;
                this.Name = level.ToString();
                this.Display = string.IsNullOrWhiteSpace(display) ? this.Name.Substring(0, 1) : display;
                this.LowRange = lowRange;
                this.HighRange = highRange;
            }

            public Licenses Level { get; protected set; }
            public string Name { get; protected set; }
            public string Display { get; protected set; }
            public int LowRange { get; protected set; }
            public int HighRange { get; protected set; }

            [Serializable]
            public enum Licenses
            {
                R = 0,
                D,
                C,
                B,
                A,
                P,
                WC,
                Unknown
            }

            private static List<LicenseLevel> _licenseLevels;

            static LicenseLevel()
            {
                _licenseLevels = new List<LicenseLevel>(new LicenseLevel[]
                                                        {
                                                            new LicenseRookie(),
                                                            new LicenseD(),
                                                            new LicenseC(),
                                                            new LicenseB(),
                                                            new LicenseA(),
                                                            new LicensePro(),
                                                            new LicenseProWC(),
                                                            new LicenseUnknown()
                                                        });
            }

            public static LicenseLevel FromLevel(int level)
            {
                var license = _licenseLevels.SingleOrDefault(l => l.LowRange <= level && l.HighRange >= level);
                if (license == null) return _licenseLevels.Last(); // unknown
                return license;
            }
        }

        public class LicenseRookie : LicenseLevel
        {
            public LicenseRookie()
                : base(Licenses.R, 0, 4)
            { }
        }

        public class LicenseD : LicenseLevel
        {
            public LicenseD()
                : base(Licenses.D, 5, 8)
            { }
        }

        public class LicenseC : LicenseLevel
        {
            public LicenseC()
                : base(Licenses.C, 9, 12)
            { }
        }

        public class LicenseB : LicenseLevel
        {
            public LicenseB()
                : base(Licenses.B, 13, 16)
            { }
        }

        public class LicenseA : LicenseLevel
        {
            public LicenseA()
                : base(Licenses.A, 17, 20)
            { }
        }

        public class LicensePro : LicenseLevel
        {
            public LicensePro()
                : base(Licenses.P, 21, 24)
            {
            }
        }

        public class LicenseProWC : LicenseLevel
        {
            public LicenseProWC()
                : base(Licenses.WC, 25, 28, "WC")
            { }
        }

        public class LicenseUnknown : LicenseLevel
        {
            public LicenseUnknown()
                : base(Licenses.Unknown, -1, -1, "?")
            {
            }
        }
    }
}
