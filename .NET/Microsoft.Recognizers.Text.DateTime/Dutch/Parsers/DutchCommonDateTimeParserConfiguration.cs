using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Dutch;
using Microsoft.Recognizers.Text.DateTime.Dutch.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Dutch;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchCommonDateTimeParserConfiguration : BaseDateParserConfiguration, ICommonDateTimeParserConfiguration
    {
        public DutchCommonDateTimeParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            UtilityConfiguration = new DutchDatetimeUtilityConfiguration();

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

            CardinalExtractor = Number.Dutch.CardinalExtractor.GetInstance(numConfig);
            IntegerExtractor = Number.Dutch.IntegerExtractor.GetInstance(numConfig);
            OrdinalExtractor = Number.Dutch.OrdinalExtractor.GetInstance(numConfig);

            NumberParser = new BaseNumberParser(new DutchNumberParserConfiguration(numConfig));

            TimeZoneParser = new BaseTimeZoneParser();
            DateExtractor = new BaseDateExtractor(new DutchDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new DutchTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new DutchDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new DutchDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new DutchDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new DutchTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new DutchDateTimePeriodExtractorConfiguration(this));
            DurationParser = new BaseDurationParser(new DutchDurationParserConfiguration(this));
            DateParser = new BaseDateParser(new DutchDateParserConfiguration(this));
            TimeParser = new TimeParser(new DutchTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new DutchDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new DutchDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new DutchTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new DutchDateTimePeriodParserConfiguration(this));
            DateTimeAltParser = new BaseDateTimeAltParser(new DutchDateTimeAltParserConfiguration(this));
        }

        public override IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary().AddRange(DateTimeDefinitions.DayOfMonth);
    }
}