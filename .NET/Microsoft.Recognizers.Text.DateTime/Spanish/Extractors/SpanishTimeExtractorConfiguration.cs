using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishTimeExtractorConfiguration : BaseOptionsConfiguration, ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------------
        public static readonly Regex DescRegex = new Regex(DateTimeDefinitions.DescRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HourNumRegex = new Regex(DateTimeDefinitions.HourNumRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MinuteNumRegex = new Regex(DateTimeDefinitions.MinuteNumRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // part 2: middle level component
        // --------------------------------------
        // handle "... en punto"
        public static readonly Regex OclockRegex = new Regex(DateTimeDefinitions.OclockRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "... tarde"
        public static readonly Regex PmRegex = new Regex(DateTimeDefinitions.PmRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "... de la mañana"
        public static readonly Regex AmRegex = new Regex(DateTimeDefinitions.AmRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "y media ..." "menos cuarto ..."
        public static readonly Regex LessThanOneHour = new Regex(DateTimeDefinitions.LessThanOneHour, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TensTimeRegex = new Regex(DateTimeDefinitions.TensTimeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "seis treinta", "seis veintiuno", "seis menos diez"
        public static readonly Regex WrittenTimeRegex = new Regex(DateTimeDefinitions.WrittenTimeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimePrefix = new Regex(DateTimeDefinitions.TimePrefix, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeSuffix = new Regex(DateTimeDefinitions.TimeSuffix, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BasicTime = new Regex(DateTimeDefinitions.BasicTime, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // part 3: regex for time
        // --------------------------------------
        // handle "a las cuatro" "a las 3"
        //TODO: add some new regex which have used in AtRegex
        //TODO: modify according to corresponding English regex
        public static readonly Regex AtRegex = new Regex(DateTimeDefinitions.AtRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ConnectNumRegex = new Regex(DateTimeDefinitions.ConnectNumRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeBeforeAfterRegex = new Regex(DateTimeDefinitions.TimeBeforeAfterRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] TimeRegexList =
        {
            // (tres min pasadas las)? siete|7|(siete treinta) pm
            new Regex(DateTimeDefinitions.TimeRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (tres min pasadas las)? 3:00(:00)? (pm)?
            new Regex(DateTimeDefinitions.TimeRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (tres min pasadas las)? 3.00 (pm)
            new Regex(DateTimeDefinitions.TimeRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline),
            
            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex4, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            new Regex(DateTimeDefinitions.TimeRegex5, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            new Regex(DateTimeDefinitions.TimeRegex6, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (En la noche) a las (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex7, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (En la noche) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex8, RegexOptions.IgnoreCase | RegexOptions.Singleline),
            
            // once (y)? veinticinco
            new Regex(DateTimeDefinitions.TimeRegex9, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            new Regex(DateTimeDefinitions.TimeRegex10, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (tres menos veinte) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex11, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (tres min pasadas las)? 3h00 (pm)?
            new Regex(DateTimeDefinitions.TimeRegex12, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 340pm
            ConnectNumRegex
        };

        Regex ITimeExtractorConfiguration.IshRegex => null;

        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;

        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;

        Regex ITimeExtractorConfiguration.TimeBeforeAfterRegex => TimeBeforeAfterRegex;

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public SpanishTimeExtractorConfiguration(DateTimeOptions options = DateTimeOptions.None) : base(options)
        {
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
            TimeZoneExtractor = new BaseTimeZoneExtractor(new SpanishTimeZoneExtractorConfiguration());
        }
    }
}
