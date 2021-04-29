using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseMergedExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKMergedExtractorConfiguration
    {
        public static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.ParserConfigurationBefore, RegexFlags);
        public static readonly Regex AfterRegex = new Regex(DateTimeDefinitions.ParserConfigurationAfter, RegexFlags);
        public static readonly Regex UntilRegex = new Regex(DateTimeDefinitions.ParserConfigurationUntil, RegexFlags);
        public static readonly Regex SincePrefixRegex = new Regex(DateTimeDefinitions.ParserConfigurationSincePrefix, RegexFlags);
        public static readonly Regex SinceSuffixRegex = new Regex(DateTimeDefinitions.ParserConfigurationSinceSuffix, RegexFlags);
        public static readonly Regex EqualRegex = new Regex(BaseDateTime.EqualRegex, RegexFlags);
        public static readonly Regex PotentialAmbiguousRangeRegex = new Regex(DateTimeDefinitions.FromToRegex, RegexFlags);
        public static readonly Regex AmbiguousRangeModifierPrefix = new Regex(DateTimeDefinitions.AmbiguousRangeModifierPrefix, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public JapaneseMergedExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(DateTimeDefinitions.AmbiguityFiltersDict);

            DateExtractor = new BaseCJKDateExtractor(new JapaneseDateExtractorConfiguration(this));
            TimeExtractor = new BaseCJKTimeExtractor(new JapaneseTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseCJKDateTimeExtractor(new JapaneseDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseCJKDatePeriodExtractor(new JapaneseDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseCJKTimePeriodExtractor(new JapaneseTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseCJKDateTimePeriodExtractor(new JapaneseDateTimePeriodExtractorConfiguration(this));
            DurationExtractor = new BaseCJKDurationExtractor(new JapaneseDurationExtractorConfiguration(this));
            SetExtractor = new BaseCJKSetExtractor(new JapaneseSetExtractorConfiguration(this));
            HolidayExtractor = new BaseCJKHolidayExtractor(new JapaneseHolidayExtractorConfiguration(this));
        }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor DateTimePeriodExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor SetExtractor { get; }

        public IDateTimeExtractor HolidayExtractor { get; }

        Regex ICJKMergedExtractorConfiguration.AfterRegex => AfterRegex;

        Regex ICJKMergedExtractorConfiguration.BeforeRegex => BeforeRegex;

        Regex ICJKMergedExtractorConfiguration.SincePrefixRegex => SincePrefixRegex;

        Regex ICJKMergedExtractorConfiguration.SinceSuffixRegex => SinceSuffixRegex;

        Regex ICJKMergedExtractorConfiguration.UntilRegex => UntilRegex;

        Regex ICJKMergedExtractorConfiguration.EqualRegex => EqualRegex;

        Regex ICJKMergedExtractorConfiguration.PotentialAmbiguousRangeRegex => PotentialAmbiguousRangeRegex;

        Regex ICJKMergedExtractorConfiguration.AmbiguousRangeModifierPrefix => AmbiguousRangeModifierPrefix;

        public Dictionary<Regex, Regex> AmbiguityFiltersDict { get; }
    }
}