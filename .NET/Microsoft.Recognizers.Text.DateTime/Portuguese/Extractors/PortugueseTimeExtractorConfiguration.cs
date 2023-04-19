// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Definitions.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseTimeExtractorConfiguration : BaseDateTimeOptionsConfiguration, ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------------
        public static readonly Regex DescRegex =
            new Regex(DateTimeDefinitions.DescRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex HourNumRegex =
            new Regex(DateTimeDefinitions.HourNumRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MinuteNumRegex =
            new Regex(DateTimeDefinitions.MinuteNumRegex, RegexFlags, RegexTimeOut);

        // part 2: middle level component
        // --------------------------------------
        // handle "... en punto"
        public static readonly Regex OclockRegex =
            new Regex(DateTimeDefinitions.OclockRegex, RegexFlags, RegexTimeOut);

        // handle "... tarde"
        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexFlags, RegexTimeOut);

        // handle "... de la mañana"
        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexFlags, RegexTimeOut);

        // handle "y media ..." "menos cuarto ..."
        public static readonly Regex LessThanOneHour =
            new Regex(DateTimeDefinitions.LessThanOneHour, RegexFlags, RegexTimeOut);

        public static readonly Regex TensTimeRegex =
            new Regex(DateTimeDefinitions.TensTimeRegex, RegexFlags, RegexTimeOut);

        // handle "seis treinta", "seis veintiuno", "seis menos diez"
        public static readonly Regex WrittenTimeRegex =
            new Regex(DateTimeDefinitions.WrittenTimeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimePrefix =
            new Regex(DateTimeDefinitions.TimePrefix, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeSuffix =
            new Regex(DateTimeDefinitions.TimeSuffix, RegexFlags, RegexTimeOut);

        public static readonly Regex BasicTime =
            new Regex(DateTimeDefinitions.BasicTime, RegexFlags, RegexTimeOut);

        // part 3: regex for time
        // --------------------------------------
        // handle "a las cuatro" "a las 3"
        // TODO: add some new regex which have used in AtRegex
        // TODO: modify according to corresponding English regex
        public static readonly Regex AtRegex =
            new Regex(DateTimeDefinitions.AtRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex ConnectNumRegex =
            new Regex(DateTimeDefinitions.ConnectNumRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeBeforeAfterRegex =
            new Regex(DateTimeDefinitions.TimeBeforeAfterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex[] TimeRegexList =
        {
            // (tres min pasadas las)? siete|7|(siete treinta) pm
            new Regex(DateTimeDefinitions.TimeRegex1, RegexFlags, RegexTimeOut),

            // (tres min pasadas las)? 3:00(:00)? (pm)?
            new Regex(DateTimeDefinitions.TimeRegex2, RegexFlags, RegexTimeOut),

            // (tres min pasadas las)? 3.00 (pm)
            new Regex(DateTimeDefinitions.TimeRegex3, RegexFlags, RegexTimeOut),

            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex4, RegexFlags, RegexTimeOut),

            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            new Regex(DateTimeDefinitions.TimeRegex5, RegexFlags, RegexTimeOut),

            // (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            new Regex(DateTimeDefinitions.TimeRegex6, RegexFlags, RegexTimeOut),

            // (En la noche) a las (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex7, RegexFlags, RegexTimeOut),

            // (En la noche) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex8, RegexFlags, RegexTimeOut),

            // once (y)? veinticinco
            new Regex(DateTimeDefinitions.TimeRegex9, RegexFlags, RegexTimeOut),

            // (tres menos veinte) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex11, RegexFlags, RegexTimeOut),

            // (tres min pasadas las)? 3h00 (pm)?
            new Regex(DateTimeDefinitions.TimeRegex12, RegexFlags, RegexTimeOut),

            WrittenTimeRegex,

            // 340pm
            ConnectNumRegex,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public PortugueseTimeExtractorConfiguration(IDateTimeOptionsConfiguration config)
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

        public string TimeTokenPrefix => DateTimeDefinitions.TimeTokenPrefix;

        public Dictionary<Regex, Regex> AmbiguityFiltersDict => DefinitionLoader.LoadAmbiguityFilters(DateTimeDefinitions.AmbiguityTimeFiltersDict);
    }
}
