using System.Text.RegularExpressions;
using System.Collections.Generic;

using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.Matcher;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchMergedExtractorConfiguration : BaseOptionsConfiguration, IMergedExtractorConfiguration
    {
        public static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline); // avant - 'before'

        public static readonly Regex AfterRegex =
            new Regex(DateTimeDefinitions.AfterRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline); // ensuite/puis are for adverbs, i.e 'i ate and then i walked', so we'll use apres 

        public static readonly Regex SinceRegex =
            new Regex(DateTimeDefinitions.SinceRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FromToRegex =
            new Regex(DateTimeDefinitions.FromToRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline); // 'Je vais du lundi au mecredi' - I will go from monday to weds

        public static readonly Regex SingleAmbiguousMonthRegex =
            new Regex(DateTimeDefinitions.SingleAmbiguousMonthRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PrepositionSuffixRegex =
            new Regex(DateTimeDefinitions.PrepositionSuffixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberEndingPattern =
            new Regex(DateTimeDefinitions.NumberEndingPattern,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearAfterRegex =
            new Regex(DateTimeDefinitions.YearAfterRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnspecificDatePeriodRegex =
            new Regex(DateTimeDefinitions.UnspecificDatePeriodRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] FilterWordRegexList =
        {

        };

        public static readonly StringMatcher SuperfluousWordMatcher = new StringMatcher();

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

        public IDateTimeListExtractor DateTimeAltExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public FrenchMergedExtractorConfiguration(DateTimeOptions options) : base(options)
        {

            DateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration(this));
            SetExtractor = new BaseSetExtractor(new FrenchSetExtractorConfiguration(this));
            HolidayExtractor = new BaseHolidayExtractor(new FrenchHolidayExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new FrenchTimeZoneExtractorConfiguration(this));
            DateTimeAltExtractor = new BaseDateTimeAltExtractor(new FrenchDateTimeAltExtractorConfiguration(this));
            IntegerExtractor = new Number.French.IntegerExtractor();
        }

        Regex IMergedExtractorConfiguration.AfterRegex => AfterRegex;
        Regex IMergedExtractorConfiguration.BeforeRegex => BeforeRegex;
        Regex IMergedExtractorConfiguration.SinceRegex => SinceRegex;
        Regex IMergedExtractorConfiguration.FromToRegex => FromToRegex;
        Regex IMergedExtractorConfiguration.SingleAmbiguousMonthRegex => SingleAmbiguousMonthRegex;
        Regex IMergedExtractorConfiguration.PrepositionSuffixRegex => PrepositionSuffixRegex;
        Regex IMergedExtractorConfiguration.NumberEndingPattern => NumberEndingPattern;
        Regex IMergedExtractorConfiguration.YearAfterRegex => YearAfterRegex;
        Regex IMergedExtractorConfiguration.UnspecificDatePeriodRegex => UnspecificDatePeriodRegex;
        IEnumerable<Regex> IMergedExtractorConfiguration.FilterWordRegexList => FilterWordRegexList;
        StringMatcher IMergedExtractorConfiguration.SuperfluousWordMatcher => SuperfluousWordMatcher;
    }
}
