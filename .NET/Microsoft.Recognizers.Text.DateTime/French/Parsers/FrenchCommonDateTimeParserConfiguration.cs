using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.DateTime.French.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.French;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchCommonDateTimeParserConfiguration : BaseDateParserConfiguration
    {
        public FrenchCommonDateTimeParserConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            UtilityConfiguration = new FrenchDatetimeUtilityConfiguration();

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

            CardinalExtractor = Number.French.CardinalExtractor.GetInstance();
            IntegerExtractor = Number.French.IntegerExtractor.GetInstance();
            OrdinalExtractor = Number.French.OrdinalExtractor.GetInstance();

            NumberParser = new BaseNumberParser(new FrenchNumberParserConfiguration());
            DateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration(this));

            // DurationParser should be assigned first, as DateParser would reference the DurationParser
            DurationParser = new BaseDurationParser(new FrenchDurationParserConfiguration(this));
            DateParser = new BaseDateParser(new FrenchDateParserConfiguration(this));
            TimeParser = new BaseTimeParser(new FrenchTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new FrenchDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new FrenchDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new FrenchTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new FrenchDateTimePeriodParserConfiguration(this));
            DateTimeAltParser = new BaseDateTimeAltParser(new FrenchDateTimeAltParserConfiguration(this));
        }

        public override IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary().AddRange(DateTimeDefinitions.DayOfMonth);
    }
}
