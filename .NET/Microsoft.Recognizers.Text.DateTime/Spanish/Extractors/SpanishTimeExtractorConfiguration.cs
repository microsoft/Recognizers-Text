using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishTimeExtractorConfiguration : BaseOptionsConfiguration, ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------------
        public static readonly Regex DescRegex =
            new Regex(DateTimeDefinitions.DescRegex, RegexFlags);

        public static readonly Regex HourNumRegex =
            new Regex(DateTimeDefinitions.HourNumRegex, RegexFlags);

        public static readonly Regex MinuteNumRegex =
            new Regex(DateTimeDefinitions.MinuteNumRegex, RegexFlags);

        // part 2: middle level component
        // --------------------------------------
        // handle "... en punto"
        public static readonly Regex OclockRegex =
            new Regex(DateTimeDefinitions.OclockRegex, RegexFlags);

        // handle "... tarde"
        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexFlags);

        // handle "... de la mañana"
        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexFlags);

        // handle "y media ..." "menos cuarto ..."
        public static readonly Regex LessThanOneHour =
            new Regex(DateTimeDefinitions.LessThanOneHour, RegexFlags);

        public static readonly Regex TensTimeRegex =
            new Regex(DateTimeDefinitions.TensTimeRegex, RegexFlags);

        // handle "seis treinta", "seis veintiuno", "seis menos diez"
        public static readonly Regex WrittenTimeRegex =
            new Regex(DateTimeDefinitions.WrittenTimeRegex, RegexFlags);

        public static readonly Regex TimePrefix =
            new Regex(DateTimeDefinitions.TimePrefix, RegexFlags);

        public static readonly Regex TimeSuffix =
            new Regex(DateTimeDefinitions.TimeSuffix, RegexFlags);

        public static readonly Regex BasicTime =
            new Regex(DateTimeDefinitions.BasicTime, RegexFlags);

        // part 3: regex for time
        // --------------------------------------
        // handle "a las cuatro" "a las 3"
        // TODO: add some new regex which have used in AtRegex
        // TODO: modify according to corresponding English regex
        public static readonly Regex AtRegex =
            new Regex(DateTimeDefinitions.AtRegex, RegexFlags);

        public static readonly Regex ConnectNumRegex =
            new Regex(DateTimeDefinitions.ConnectNumRegex, RegexFlags);

        public static readonly Regex TimeBeforeAfterRegex =
            new Regex(DateTimeDefinitions.TimeBeforeAfterRegex, RegexFlags);

        public static readonly Regex[] TimeRegexList =
        {
            // (tres min pasadas las)? siete|7|(siete treinta) pm
            new Regex(DateTimeDefinitions.TimeRegex1, RegexFlags),

            // (tres min pasadas las)? 3:00(:00)? (pm)?
            new Regex(DateTimeDefinitions.TimeRegex2, RegexFlags),

            // (tres min pasadas las)? 3.00 (pm)
            new Regex(DateTimeDefinitions.TimeRegex3, RegexFlags),

            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex4, RegexFlags),

            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            new Regex(DateTimeDefinitions.TimeRegex5, RegexFlags),

            // (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            new Regex(DateTimeDefinitions.TimeRegex6, RegexFlags),

            // (En la noche) a las (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex7, RegexFlags),

            // (En la noche) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex8, RegexFlags),

            // once (y)? veinticinco
            new Regex(DateTimeDefinitions.TimeRegex9, RegexFlags),

            new Regex(DateTimeDefinitions.TimeRegex10, RegexFlags),

            // (tres menos veinte) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex11, RegexFlags),

            // (tres min pasadas las)? 3h00 (pm)?
            new Regex(DateTimeDefinitions.TimeRegex12, RegexFlags),

            // 340pm
            ConnectNumRegex,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public SpanishTimeExtractorConfiguration(IOptionsConfiguration config)
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
    }
}
