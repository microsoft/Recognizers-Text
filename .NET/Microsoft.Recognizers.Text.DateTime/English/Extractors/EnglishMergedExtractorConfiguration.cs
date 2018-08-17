using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishMergedExtractorConfiguration : BaseOptionsConfiguration, IMergedExtractorConfiguration
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

        public static readonly Regex YearAfterRegex =
            new Regex(DateTimeDefinitions.YearAfterRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnspecificDatePeriodRegex =
            new Regex(DateTimeDefinitions.UnspecificDatePeriodRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] FilterWordRegexList =
        {
            // one on one
            new Regex(DateTimeDefinitions.OneOnOneRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline),
            // (the)? (day|week|month|year)
            new Regex(DateTimeDefinitions.SingleAmbiguousTermsRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
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

        public Dictionary<Regex, Regex> AmbiguityFiltersDict { get; }

        public EnglishMergedExtractorConfiguration(IOptionsConfiguration config) : base(config)
        {
            DateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(this));
            SetExtractor = new BaseSetExtractor(new EnglishSetExtractorConfiguration(this));
            HolidayExtractor = new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new EnglishTimeZoneExtractorConfiguration(this));
            DateTimeAltExtractor = new BaseDateTimeAltExtractor(new EnglishDateTimeAltExtractorConfiguration(this));
            IntegerExtractor = Number.English.IntegerExtractor.GetInstance();

            if ((Options & DateTimeOptions.EnablePreview) != 0)
            {
                SuperfluousWordMatcher.Init(DateTimeDefinitions.SuperfluousWordList);
            }

            var ambiguityFiltersDict = new Dictionary<Regex, Regex>();

            foreach (var item in DateTimeDefinitions.AmbiguityFiltersDict)
            {
                ambiguityFiltersDict.Add(new Regex(item.Key, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    new Regex(item.Value, RegexOptions.IgnoreCase | RegexOptions.Singleline));
            }

            AmbiguityFiltersDict = ambiguityFiltersDict;

        }

        Regex IMergedExtractorConfiguration.AfterRegex => AfterRegex;
        Regex IMergedExtractorConfiguration.BeforeRegex => BeforeRegex;
        Regex IMergedExtractorConfiguration.SinceRegex => SinceRegex;
        Regex IMergedExtractorConfiguration.AroundRegex => AroundRegex;
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
