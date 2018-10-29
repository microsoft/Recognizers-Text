using System.Text.RegularExpressions;
using System.Collections.Generic;

using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanMergedExtractorConfiguration : BaseOptionsConfiguration, IMergedExtractorConfiguration
    {
        public static readonly Regex BeforeRegex = 
            new Regex(DateTimeDefinitions.BeforeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AfterRegex = 
            new Regex(DateTimeDefinitions.AfterRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SinceRegex =
            new Regex(DateTimeDefinitions.SinceRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AroundRegex =
            new Regex(DateTimeDefinitions.AroundRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FromToRegex = 
            new Regex(DateTimeDefinitions.FromToRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SingleAmbiguousMonthRegex =
            new Regex(DateTimeDefinitions.SingleAmbiguousMonthRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PrepositionSuffixRegex =
            new Regex(DateTimeDefinitions.PrepositionSuffixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberEndingPattern =
            new Regex(DateTimeDefinitions.NumberEndingPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DateAfterRegex =
            new Regex(DateTimeDefinitions.YearAfterRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnspecificDatePeriodRegex =
            new Regex(DateTimeDefinitions.UnspecificDatePeriodRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly StringMatcher SuperfluousWordMatcher = new StringMatcher();

        public static readonly Regex[] TermFilterRegexes =
        {
            // one on one
            new Regex(DateTimeDefinitions.OneOnOneRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline),
        };

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeExtractor DateTimePeriodExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor SetExtractor { get; }

        public IDateTimeExtractor HolidayExtractor { get; }

        public IDateTimeZoneExtractor TimeZoneExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public IDateTimeListExtractor DateTimeAltExtractor { get; }

        public Dictionary<Regex, Regex> AmbiguityFiltersDict { get; } = null;

        public GermanMergedExtractorConfiguration(DateTimeOptions options) : base(options)
        {
            DateExtractor = new BaseDateExtractor(new GermanDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new GermanTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new GermanDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new GermanDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new GermanTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new GermanDateTimePeriodExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration(this));
            SetExtractor = new BaseSetExtractor(new GermanSetExtractorConfiguration(this));
            HolidayExtractor = new BaseHolidayExtractor(new GermanHolidayExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new GermanTimeZoneExtractorConfiguration(this));
            IntegerExtractor = Number.German.IntegerExtractor.GetInstance();
            DateTimeAltExtractor = new BaseDateTimeAltExtractor(new GermanDateTimeAltExtractorConfiguration(this));
        }

        Regex IMergedExtractorConfiguration.AfterRegex => AfterRegex;
        Regex IMergedExtractorConfiguration.BeforeRegex => BeforeRegex;
        Regex IMergedExtractorConfiguration.SinceRegex => SinceRegex;
        Regex IMergedExtractorConfiguration.AroundRegex => AroundRegex;
        Regex IMergedExtractorConfiguration.FromToRegex => FromToRegex;
        Regex IMergedExtractorConfiguration.SingleAmbiguousMonthRegex => SingleAmbiguousMonthRegex;
        Regex IMergedExtractorConfiguration.PrepositionSuffixRegex => PrepositionSuffixRegex;
        Regex IMergedExtractorConfiguration.NumberEndingPattern => NumberEndingPattern;
        Regex IMergedExtractorConfiguration.DateAfterRegex => DateAfterRegex;
        Regex IMergedExtractorConfiguration.UnspecificDatePeriodRegex => UnspecificDatePeriodRegex;
        IEnumerable<Regex> IMergedExtractorConfiguration.TermFilterRegexes => TermFilterRegexes;
        StringMatcher IMergedExtractorConfiguration.SuperfluousWordMatcher => SuperfluousWordMatcher;
    }
}
