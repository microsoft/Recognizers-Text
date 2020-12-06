using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishTimeExtractorConfiguration : BaseDateTimeOptionsConfiguration, ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------------
        public static readonly Regex DescRegex =
            RegexCache.Get(DateTimeDefinitions.DescRegex, RegexFlags);

        public static readonly Regex HourNumRegex =
            RegexCache.Get(DateTimeDefinitions.HourNumRegex, RegexFlags);

        public static readonly Regex MinuteNumRegex =
            RegexCache.Get(DateTimeDefinitions.MinuteNumRegex, RegexFlags);

        // part 2: middle level component
        // --------------------------------------
        // handle "... en punto"
        public static readonly Regex OclockRegex =
            RegexCache.Get(DateTimeDefinitions.OclockRegex, RegexFlags);

        // handle "... tarde"
        public static readonly Regex PmRegex =
            RegexCache.Get(DateTimeDefinitions.PmRegex, RegexFlags);

        // handle "... de la mañana"
        public static readonly Regex AmRegex =
            RegexCache.Get(DateTimeDefinitions.AmRegex, RegexFlags);

        // handle "y media ..." "menos cuarto ..."
        public static readonly Regex LessThanOneHour =
            RegexCache.Get(DateTimeDefinitions.LessThanOneHour, RegexFlags);

        public static readonly Regex TensTimeRegex =
            RegexCache.Get(DateTimeDefinitions.TensTimeRegex, RegexFlags);

        // handle "seis treinta", "seis veintiuno", "seis menos diez"
        public static readonly Regex WrittenTimeRegex =
            RegexCache.Get(DateTimeDefinitions.WrittenTimeRegex, RegexFlags);

        public static readonly Regex TimePrefix =
            RegexCache.Get(DateTimeDefinitions.TimePrefix, RegexFlags);

        public static readonly Regex TimeSuffix =
            RegexCache.Get(DateTimeDefinitions.TimeSuffix, RegexFlags);

        public static readonly Regex BasicTime =
            RegexCache.Get(DateTimeDefinitions.BasicTime, RegexFlags);

        // part 3: regex for time
        // --------------------------------------
        // handle "a las cuatro" "a las 3"
        // TODO: add some new regex which have used in AtRegex
        // TODO: modify according to corresponding English regex
        public static readonly Regex AtRegex =
            RegexCache.Get(DateTimeDefinitions.AtRegex, RegexFlags);

        public static readonly Regex ConnectNumRegex =
            RegexCache.Get(DateTimeDefinitions.ConnectNumRegex, RegexFlags);

        public static readonly Regex TimeBeforeAfterRegex =
            RegexCache.Get(DateTimeDefinitions.TimeBeforeAfterRegex, RegexFlags);

        public static readonly Regex[] TimeRegexList =
        {
            // (tres min pasadas las)? siete|7|(siete treinta) pm
            RegexCache.Get(DateTimeDefinitions.TimeRegex1, RegexFlags),

            // (tres min pasadas las)? 3:00(:00)? (pm)?
            RegexCache.Get(DateTimeDefinitions.TimeRegex2, RegexFlags),

            // (tres min pasadas las)? 3.00 (pm)
            RegexCache.Get(DateTimeDefinitions.TimeRegex3, RegexFlags),

            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            RegexCache.Get(DateTimeDefinitions.TimeRegex4, RegexFlags),

            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            RegexCache.Get(DateTimeDefinitions.TimeRegex5, RegexFlags),

            // (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            RegexCache.Get(DateTimeDefinitions.TimeRegex6, RegexFlags),

            // (En la noche) a las (cinco treinta|siete|7|7:00(:00)?) (pm)?
            RegexCache.Get(DateTimeDefinitions.TimeRegex7, RegexFlags),

            // (En la noche) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            RegexCache.Get(DateTimeDefinitions.TimeRegex8, RegexFlags),

            // once (y)? veinticinco
            RegexCache.Get(DateTimeDefinitions.TimeRegex9, RegexFlags),

            RegexCache.Get(DateTimeDefinitions.TimeRegex10, RegexFlags),

            // (tres menos veinte) (pm)?
            RegexCache.Get(DateTimeDefinitions.TimeRegex11, RegexFlags),

            // (tres min pasadas las)? 3h00 (pm)?
            RegexCache.Get(DateTimeDefinitions.TimeRegex12, RegexFlags),

            // 340pm
            ConnectNumRegex,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public SpanishTimeExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new SpanishTimeZoneExtractorConfiguration(this));
        }

        Regex ITimeExtractorConfiguration.IshRegex => null;

        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;

        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;

        Regex ITimeExtractorConfiguration.TimeBeforeAfterRegex => TimeBeforeAfterRegex;

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public string TimeTokenPrefix => DateTimeDefinitions.TimeTokenPrefix;

        public Dictionary<Regex, Regex> AmbiguityFiltersDict => null;
    }
}
