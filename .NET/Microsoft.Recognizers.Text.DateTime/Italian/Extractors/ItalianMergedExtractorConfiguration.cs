using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianMergedExtractorConfiguration : BaseOptionsConfiguration, IMergedExtractorConfiguration
    {
        public static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex, RegexOptions.Singleline); // avant - 'before'

        public static readonly Regex AfterRegex =
            new Regex(DateTimeDefinitions.AfterRegex, RegexOptions.Singleline); // ensuite/puis are for adverbs, i.e 'i ate and then i walked', so we'll use apres

        public static readonly Regex SinceRegex =
            new Regex(DateTimeDefinitions.SinceRegex, RegexOptions.Singleline);

        public static readonly Regex AroundRegex =
            new Regex(DateTimeDefinitions.AroundRegex, RegexOptions.Singleline);

        public static readonly Regex FromToRegex =
            new Regex(DateTimeDefinitions.FromToRegex, RegexOptions.Singleline); // 'Je vais du lundi au mecredi' - I will go from monday to weds

        public static readonly Regex SingleAmbiguousMonthRegex =
            new Regex(DateTimeDefinitions.SingleAmbiguousMonthRegex, RegexOptions.Singleline);

        public static readonly Regex PrepositionSuffixRegex =
            new Regex(DateTimeDefinitions.PrepositionSuffixRegex, RegexOptions.Singleline);

        public static readonly Regex NumberEndingPattern =
            new Regex(DateTimeDefinitions.NumberEndingPattern, RegexOptions.Singleline);

        public static readonly Regex SuffixAfterRegex =
            new Regex(DateTimeDefinitions.SuffixAfterRegex, RegexOptions.Singleline);

        public static readonly Regex UnspecificDatePeriodRegex =
            new Regex(DateTimeDefinitions.UnspecificDatePeriodRegex, RegexOptions.Singleline);

        public static readonly Regex[] TermFilterRegexes =
        {
        };

        public static readonly StringMatcher SuperfluousWordMatcher = new StringMatcher();

        public ItalianMergedExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            DateExtractor = new BaseDateExtractor(new ItalianDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new ItalianTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new ItalianDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new ItalianDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new ItalianTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new ItalianDateTimePeriodExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new ItalianDurationExtractorConfiguration(this));
            SetExtractor = new BaseSetExtractor(new ItalianSetExtractorConfiguration(this));
            HolidayExtractor = new BaseHolidayExtractor(new ItalianHolidayExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new ItalianTimeZoneExtractorConfiguration(this));
            DateTimeAltExtractor = new BaseDateTimeAltExtractor(new ItalianDateTimeAltExtractorConfiguration(this));
            IntegerExtractor = Number.Italian.IntegerExtractor.GetInstance();
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

        public Dictionary<Regex, Regex> AmbiguityFiltersDict { get; } = null;

        Regex IMergedExtractorConfiguration.AfterRegex => AfterRegex;

        Regex IMergedExtractorConfiguration.BeforeRegex => BeforeRegex;

        Regex IMergedExtractorConfiguration.SinceRegex => SinceRegex;

        Regex IMergedExtractorConfiguration.AroundRegex => AroundRegex;

        Regex IMergedExtractorConfiguration.FromToRegex => FromToRegex;

        Regex IMergedExtractorConfiguration.SingleAmbiguousMonthRegex => SingleAmbiguousMonthRegex;

        Regex IMergedExtractorConfiguration.PrepositionSuffixRegex => PrepositionSuffixRegex;

        Regex IMergedExtractorConfiguration.NumberEndingPattern => NumberEndingPattern;

        Regex IMergedExtractorConfiguration.SuffixAfterRegex => SuffixAfterRegex;

        Regex IMergedExtractorConfiguration.UnspecificDatePeriodRegex => UnspecificDatePeriodRegex;

        IEnumerable<Regex> IMergedExtractorConfiguration.TermFilterRegexes => TermFilterRegexes;

        StringMatcher IMergedExtractorConfiguration.SuperfluousWordMatcher => SuperfluousWordMatcher;
    }
}
