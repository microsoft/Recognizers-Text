using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.DateTime.English.Utilities;
using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishCommonDateTimeParserConfiguration : BaseDateParserConfiguration, ICommonDateTimeParserConfiguration
    {

        public new static readonly Regex AmbiguousMonthP0Regex =
            new Regex(DateTimeDefinitions.AmbiguousMonthP0Regex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public EnglishCommonDateTimeParserConfiguration(DateTimeOptions options) : base(options)
        {
            UtilityConfiguration = new EnglishDatetimeUtilityConfiguration();

            UnitMap = DateTimeDefinitions.UnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.UnitValueMap.ToImmutableDictionary();
            SeasonMap = DateTimeDefinitions.SeasonMap.ToImmutableDictionary();
            CardinalMap = DateTimeDefinitions.CardinalMap.ToImmutableDictionary();
            DayOfWeek = DateTimeDefinitions.DayOfWeek.ToImmutableDictionary();
            MonthOfYear = DateTimeDefinitions.MonthOfYear.ToImmutableDictionary();
            Numbers = DateTimeDefinitions.Numbers.ToImmutableDictionary();
            DoubleNumbers = DateTimeDefinitions.DoubleNumbers.ToImmutableDictionary();
            WrittenDecades = DateTimeDefinitions.WrittenDecades.ToImmutableDictionary();
            SpecialDecadeCases = DateTimeDefinitions.SpecialDecadeCases.ToImmutableDictionary();

            CardinalExtractor = Number.English.CardinalExtractor.GetInstance();
            IntegerExtractor = Number.English.IntegerExtractor.GetInstance();
            OrdinalExtractor = Number.English.OrdinalExtractor.GetInstance();

            NumberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
            DateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(options));
            DateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration(options));
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(options));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration(options));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration(options));
            DurationParser = new BaseDurationParser(new EnglishDurationParserConfiguration(this));
            DateParser = new BaseDateParser(new EnglishDateParserConfiguration(this));
            TimeParser = new TimeParser(new EnglishTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new EnglishDateTimePeriodParserConfiguration(this));
            DateTimeAltParser = new BaseDateTimeAltParser(new EnglishDateTimeAltParserConfiguration(this));
        }

        Regex ICommonDateTimeParserConfiguration.AmbiguousMonthP0Regex => AmbiguousMonthP0Regex;

        public override IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary().AddRange(DateTimeDefinitions.DayOfMonth);
    }
}