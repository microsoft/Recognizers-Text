using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.DateTime.German.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.German;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanCommonDateTimeParserConfiguration : BaseDateParserConfiguration
    {
        public GermanCommonDateTimeParserConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            UtilityConfiguration = new GermanDatetimeUtilityConfiguration();

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

            CardinalExtractor = Number.German.CardinalExtractor.GetInstance();
            IntegerExtractor = Number.German.IntegerExtractor.GetInstance();
            OrdinalExtractor = Number.German.OrdinalExtractor.GetInstance();

            NumberParser = new BaseNumberParser(new GermanNumberParserConfiguration());
            DateExtractor = new BaseDateExtractor(new GermanDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new GermanTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new GermanDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new GermanDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new GermanTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new GermanDateTimePeriodExtractorConfiguration(this));
            DurationParser = new BaseDurationParser(new GermanDurationParserConfiguration(this));
            DateParser = new BaseDateParser(new GermanDateParserConfiguration(this));
            TimeParser = new TimeParser(new GermanTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new GermanDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new GermanDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new GermanTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new GermanDateTimePeriodParserConfiguration(this));
        }

        public override IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary().AddRange(DateTimeDefinitions.DayOfMonth);
    }
}