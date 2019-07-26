using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Turkish;
using Microsoft.Recognizers.Text.DateTime.Turkish.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Turkish;

namespace Microsoft.Recognizers.Text.DateTime.Turkish
{
    public class TurkishCommonDateTimeParserConfiguration : BaseDateParserConfiguration, ICommonDateTimeParserConfiguration
    {
        public TurkishCommonDateTimeParserConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            UtilityConfiguration = new TurkishDatetimeUtilityConfiguration();

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

            CardinalExtractor = Number.Turkish.CardinalExtractor.GetInstance();
            IntegerExtractor = Number.Turkish.IntegerExtractor.GetInstance();
            OrdinalExtractor = Number.Turkish.OrdinalExtractor.GetInstance();

            TimeZoneParser = new BaseTimeZoneParser();
            NumberParser = new BaseNumberParser(new TurkishNumberParserConfiguration());
            DateExtractor = new BaseDateExtractor(new TurkishDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new TurkishTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new TurkishDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new TurkishDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new TurkishDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new TurkishTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new TurkishDateTimePeriodExtractorConfiguration(this));
            DurationParser = new BaseDurationParser(new TurkishDurationParserConfiguration(this));
            DateParser = new BaseDateParser(new TurkishDateParserConfiguration(this));
            TimeParser = new TimeParser(new TurkishTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new TurkishDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new TurkishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new TurkishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new TurkishDateTimePeriodParserConfiguration(this));
            DateTimeAltParser = new BaseDateTimeAltParser(new TurkishDateTimeAltParserConfiguration(this));
        }

        public override IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary().AddRange(DateTimeDefinitions.DayOfMonth);
    }
}