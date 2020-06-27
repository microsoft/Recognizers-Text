using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Hindi;
using Microsoft.Recognizers.Definitions.Utilities;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Hindi
{
    public class HindiMergedExtractorConfiguration : BaseDateTimeOptionsConfiguration, IMergedExtractorConfiguration
    {
        public static readonly Regex BeforeRegex =
           new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex AfterRegex =
            new Regex(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex SinceRegex =
            new Regex(DateTimeDefinitions.SinceRegex, RegexFlags);

        public static readonly Regex AroundRegex =
            new Regex(DateTimeDefinitions.AroundRegex, RegexFlags);

        public static readonly Regex EqualRegex =
            new Regex(BaseDateTime.EqualRegex, RegexFlags);

        public static readonly Regex FromToRegex =
            new Regex(DateTimeDefinitions.FromToRegex, RegexFlags);

        public static readonly Regex SingleAmbiguousMonthRegex =
            new Regex(DateTimeDefinitions.SingleAmbiguousMonthRegex, RegexFlags);

        public static readonly Regex PrepositionSuffixRegex =
            new Regex(DateTimeDefinitions.PrepositionSuffixRegex, RegexFlags);

        public static readonly Regex AmbiguousRangeModifierPrefix =
            new Regex(DateTimeDefinitions.AmbiguousRangeModifierPrefix, RegexFlags);

        public static readonly Regex NumberEndingPattern =
            new Regex(DateTimeDefinitions.NumberEndingPattern, RegexFlags);

        public static readonly Regex SuffixAfterRegex =
            new Regex(DateTimeDefinitions.SuffixAfterRegex, RegexFlags);

        public static readonly Regex UnspecificDatePeriodRegex =
            new Regex(DateTimeDefinitions.UnspecificDatePeriodRegex, RegexFlags);

        public static readonly Regex FailFastRegex =
            new Regex(DateTimeDefinitions.FailFastRegex, RegexFlags | RegexOptions.Compiled);

        public static readonly Regex[] TermFilterRegexes =
        {
            // one on one
            new Regex(DateTimeDefinitions.OneOnOneRegex, RegexFlags),

            // (the)? (day|week|month|year)
            new Regex(DateTimeDefinitions.SingleAmbiguousTermsRegex, RegexFlags),
        };

        public static readonly StringMatcher SuperfluousWordMatcher = new StringMatcher();

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public HindiMergedExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DateExtractor = new BaseDateExtractor(new HindiDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new HindiTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new HindiDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new HindiDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new HindiTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new HindiDateTimePeriodExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new HindiDurationExtractorConfiguration(this));
            SetExtractor = new BaseSetExtractor(new HindiSetExtractorConfiguration(this));
            HolidayExtractor = new BaseHolidayExtractor(new HindiHolidayExtractorConfiguration(this));
            DateTimeAltExtractor = new BaseDateTimeAltExtractor(new HindiDateTimeAltExtractorConfiguration(this));
            IntegerExtractor = Number.Hindi.IntegerExtractor.GetInstance();

            AmbiguityFiltersDict = DefinitionLoader.LoadAmbiguityFilters(DateTimeDefinitions.AmbiguityFiltersDict);

            if ((Options & DateTimeOptions.EnablePreview) != 0)
            {
                SuperfluousWordMatcher.Init(DateTimeDefinitions.SuperfluousWordList);
            }
        }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor DateTimePeriodExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor SetExtractor { get; }

        public IDateTimeExtractor HolidayExtractor { get; }

        public IDateTimeZoneExtractor TimeZoneExtractor { get; }

        public IDateTimeListExtractor DateTimeAltExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public Dictionary<Regex, Regex> AmbiguityFiltersDict { get; }

        Regex IMergedExtractorConfiguration.AfterRegex => AfterRegex;

        Regex IMergedExtractorConfiguration.BeforeRegex => BeforeRegex;

        Regex IMergedExtractorConfiguration.SinceRegex => SinceRegex;

        Regex IMergedExtractorConfiguration.AroundRegex => AroundRegex;

        Regex IMergedExtractorConfiguration.EqualRegex => EqualRegex;

        Regex IMergedExtractorConfiguration.FromToRegex => FromToRegex;

        Regex IMergedExtractorConfiguration.SingleAmbiguousMonthRegex => SingleAmbiguousMonthRegex;

        Regex IMergedExtractorConfiguration.PrepositionSuffixRegex => PrepositionSuffixRegex;

        Regex IMergedExtractorConfiguration.AmbiguousRangeModifierPrefix => AmbiguousRangeModifierPrefix;

        Regex IMergedExtractorConfiguration.PotentialAmbiguousRangeRegex => FromToRegex;

        Regex IMergedExtractorConfiguration.NumberEndingPattern => NumberEndingPattern;

        Regex IMergedExtractorConfiguration.SuffixAfterRegex => SuffixAfterRegex;

        Regex IMergedExtractorConfiguration.UnspecificDatePeriodRegex => UnspecificDatePeriodRegex;

        Regex IMergedExtractorConfiguration.UnspecificTimePeriodRegex => null;

        Regex IMergedExtractorConfiguration.FailFastRegex => FailFastRegex;

        IEnumerable<Regex> IMergedExtractorConfiguration.TermFilterRegexes => TermFilterRegexes;

        StringMatcher IMergedExtractorConfiguration.SuperfluousWordMatcher => SuperfluousWordMatcher;

        bool IMergedExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;
    }
}
