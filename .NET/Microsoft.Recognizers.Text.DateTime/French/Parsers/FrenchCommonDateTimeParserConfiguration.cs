using System.Collections.Immutable;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Text.DateTime.French.Utilities;
using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.Number.French;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchCommonDateTimeParserConfiguration : BaseDateParserConfiguration
    {
        public FrenchCommonDateTimeParserConfiguration(DateTimeOptions options) : base(options)
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

            CardinalExtractor = new CardinalExtractor();
            IntegerExtractor = new IntegerExtractor();
            OrdinalExtractor = new OrdinalExtractor();

            NumberParser = new BaseNumberParser(new FrenchNumberParserConfiguration());
            DateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new FrenchDateTimePeriodExtractorConfiguration());
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
