using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseTimeExtractorConfiguration : BaseOptionsConfiguration, ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------------
        public static readonly Regex DescRegex = new Regex(DateTimeDefinitions.DescRegex, RegexOptions.Singleline);

        public static readonly Regex HourNumRegex = new Regex(DateTimeDefinitions.HourNumRegex, RegexOptions.Singleline);

        public static readonly Regex MinuteNumRegex = new Regex(DateTimeDefinitions.MinuteNumRegex, RegexOptions.Singleline);

        // part 2: middle level component
        // --------------------------------------
        // handle "... en punto"
        public static readonly Regex OclockRegex = new Regex(DateTimeDefinitions.OclockRegex, RegexOptions.Singleline);

        // handle "... tarde"
        public static readonly Regex PmRegex = new Regex(DateTimeDefinitions.PmRegex, RegexOptions.Singleline);

        // handle "... de la mañana"
        public static readonly Regex AmRegex = new Regex(DateTimeDefinitions.AmRegex, RegexOptions.Singleline);

        // handle "y media ..." "menos cuarto ..."
        public static readonly Regex LessThanOneHour = new Regex(DateTimeDefinitions.LessThanOneHour, RegexOptions.Singleline);

        public static readonly Regex TensTimeRegex = new Regex(DateTimeDefinitions.TensTimeRegex, RegexOptions.Singleline);

        // handle "seis treinta", "seis veintiuno", "seis menos diez"
        public static readonly Regex WrittenTimeRegex = new Regex(DateTimeDefinitions.WrittenTimeRegex, RegexOptions.Singleline);

        public static readonly Regex TimePrefix = new Regex(DateTimeDefinitions.TimePrefix, RegexOptions.Singleline);

        public static readonly Regex TimeSuffix = new Regex(DateTimeDefinitions.TimeSuffix, RegexOptions.Singleline);

        public static readonly Regex BasicTime = new Regex(DateTimeDefinitions.BasicTime, RegexOptions.Singleline);

        // part 3: regex for time
        // --------------------------------------
        // handle "a las cuatro" "a las 3"
        // TODO: add some new regex which have used in AtRegex
        // TODO: modify according to corresponding English regex
        public static readonly Regex AtRegex = new Regex(DateTimeDefinitions.AtRegex, RegexOptions.Singleline);

        public static readonly Regex ConnectNumRegex = new Regex(DateTimeDefinitions.ConnectNumRegex, RegexOptions.Singleline);

        public static readonly Regex TimeBeforeAfterRegex = new Regex(DateTimeDefinitions.TimeBeforeAfterRegex, RegexOptions.Singleline);

        public static readonly Regex[] TimeRegexList =
        {
            // (tres min pasadas las)? siete|7|(siete treinta) pm
            new Regex(DateTimeDefinitions.TimeRegex1, RegexOptions.Singleline),

            // (tres min pasadas las)? 3:00(:00)? (pm)?
            new Regex(DateTimeDefinitions.TimeRegex2, RegexOptions.Singleline),

            // (tres min pasadas las)? 3.00 (pm)
            new Regex(DateTimeDefinitions.TimeRegex3, RegexOptions.Singleline),

            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex4, RegexOptions.Singleline),

            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            new Regex(DateTimeDefinitions.TimeRegex5, RegexOptions.Singleline),

            // (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            new Regex(DateTimeDefinitions.TimeRegex6, RegexOptions.Singleline),

            // (En la noche) a las (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex7, RegexOptions.Singleline),

            // (En la noche) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex8, RegexOptions.Singleline),

            // once (y)? veinticinco
            new Regex(DateTimeDefinitions.TimeRegex9, RegexOptions.Singleline),

            new Regex(DateTimeDefinitions.TimeRegex10, RegexOptions.Singleline),

            // (tres menos veinte) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex11, RegexOptions.Singleline),

            // (tres min pasadas las)? 3h00 (pm)?
            new Regex(DateTimeDefinitions.TimeRegex12, RegexOptions.Singleline),

            WrittenTimeRegex,

            // 340pm
            ConnectNumRegex,
        };

        public PortugueseTimeExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            DurationExtractor = new BaseDurationExtractor(new PortugueseDurationExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new PortugueseTimeZoneExtractorConfiguration(this));
        }

        Regex ITimeExtractorConfiguration.IshRegex => null;

        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;

        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;

        Regex ITimeExtractorConfiguration.TimeBeforeAfterRegex => TimeBeforeAfterRegex;

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }
    }
}
