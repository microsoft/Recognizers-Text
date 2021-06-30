using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanMergedExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKMergedExtractorConfiguration
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

        public KoreanMergedExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(DateTimeDefinitions.AmbiguityFiltersDict);

            DateExtractor = new BaseCJKDateExtractor(new KoreanDateExtractorConfiguration(this));
            TimeExtractor = new BaseCJKTimeExtractor(new KoreanTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseCJKDateTimeExtractor(new KoreanDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseCJKDatePeriodExtractor(new KoreanDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseCJKTimePeriodExtractor(new KoreanTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseCJKDateTimePeriodExtractor(new KoreanDateTimePeriodExtractorConfiguration(this));
            DurationExtractor = new BaseCJKDurationExtractor(new KoreanDurationExtractorConfiguration(this));
            SetExtractor = new BaseCJKSetExtractor(new KoreanSetExtractorConfiguration(this));
            HolidayExtractor = new BaseCJKHolidayExtractor(new KoreanHolidayExtractorConfiguration(this));
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