using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Hindi;
using Microsoft.Recognizers.Text.DateTime.Hindi.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Hindi;

namespace Microsoft.Recognizers.Text.DateTime.Hindi
{
    public class HindiCommonDateTimeParserConfiguration : BaseDateParserConfiguration, ICommonDateTimeParserConfiguration
    {
        public HindiCommonDateTimeParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            UtilityConfiguration = new HindiDatetimeUtilityConfiguration();

            UnitMap = DateTimeDefinitions.UnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.UnitValueMap.ToImmutableDictionary();
            SeasonMap = DateTimeDefinitions.SeasonMap.ToImmutableDictionary();
            SpecialYearPrefixesMap = DateTimeDefinitions.SpecialYearPrefixesMap.ToImmutableDictionary();
            CardinalMap = DateTimeDefinitions.CardinalMap.ToImmutableDictionary();
            DayOfWeek = DateTimeDefinitions.DayOfWeek.ToImmutableDictionary();
            MonthOfYear = DateTimeDefinitions.MonthOfYear.ToImmutableDictionary();
            Numbers = DateTimeDefinitions.Numbers.ToImmutableDictionary();
            DoubleNumbers = DateTimeDefinitions.DoubleNumbers.ToImmutableDictionary();
            WrittenDecades = DateTimeDefinitions.WrittenDecades.ToImmutableDictionary();
            SpecialDecadeCases = DateTimeDefinitions.SpecialDecadeCases.ToImmutableDictionary();

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = Number.Hindi.CardinalExtractor.GetInstance();
            IntegerExtractor = Number.Hindi.IntegerExtractor.GetInstance();
            OrdinalExtractor = Number.Hindi.OrdinalExtractor.GetInstance();

            NumberParser = new BaseIndianNumberParser(new HindiNumberParserConfiguration(numConfig));

            TimeZoneParser = new BaseTimeZoneParser();

            DateExtractor = new BaseDateExtractor(new HindiDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new HindiTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new HindiDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new HindiDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new HindiDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new HindiTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new HindiDateTimePeriodExtractorConfiguration(this));
            DurationParser = new BaseDurationParser(new HindiDurationParserConfiguration(this));
            DateParser = new BaseDateParser(new HindiDateParserConfiguration(this));
            TimeParser = new TimeParser(new HindiTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new HindiDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new HindiDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new HindiTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new HindiDateTimePeriodParserConfiguration(this));
            DateTimeAltParser = new BaseDateTimeAltParser(new HindiDateTimeAltParserConfiguration(this));
        }

        public override IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary().AddRange(DateTimeDefinitions.DayOfMonth);
    }
}
