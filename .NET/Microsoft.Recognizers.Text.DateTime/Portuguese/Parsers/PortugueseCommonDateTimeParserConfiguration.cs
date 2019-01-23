using System.Collections.Immutable;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Portuguese.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Portuguese;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseCommonDateTimeParserConfiguration : BaseDateParserConfiguration
    {
        public PortugueseCommonDateTimeParserConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            UtilityConfiguration = new PortugueseDatetimeUtilityConfiguration();

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

            CardinalExtractor = Number.Portuguese.CardinalExtractor.GetInstance();
            IntegerExtractor = Number.Portuguese.IntegerExtractor.GetInstance();
            OrdinalExtractor = Number.Portuguese.OrdinalExtractor.GetInstance();

            NumberParser = new BaseNumberParser(new PortugueseNumberParserConfiguration());
            DateExtractor = new BaseDateExtractor(new PortugueseDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new PortugueseTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new PortugueseDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new PortugueseDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new PortugueseDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new PortugueseTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new PortugueseDateTimePeriodExtractorConfiguration(this));
            DateParser = new BaseDateParser(new PortugueseDateParserConfiguration(this));
            TimeParser = new BaseTimeParser(new PortugueseTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new PortugueseDateTimeParserConfiguration(this));
            DurationParser = new BaseDurationParser(new PortugueseDurationParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new PortugueseDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new PortugueseTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new PortugueseDateTimePeriodParserConfiguration(this));
        }
    }
}
