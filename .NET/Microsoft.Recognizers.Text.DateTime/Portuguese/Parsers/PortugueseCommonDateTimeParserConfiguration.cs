using System.Collections.Immutable;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Portuguese.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Portuguese;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseCommonDateTimeParserConfiguration : BaseDateParserConfiguration
    {
        public PortugueseCommonDateTimeParserConfiguration(DateTimeOptions options) : base(options)
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

            CardinalExtractor = Number.Portuguese.CardinalExtractor.GetInstance();
            IntegerExtractor = new IntegerExtractor();
            OrdinalExtractor = new OrdinalExtractor();

            NumberParser = new BaseNumberParser(new PortugueseNumberParserConfiguration());
            DateExtractor = new BaseDateExtractor(new PortugueseDateExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new PortugueseTimeExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new PortugueseDateTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new PortugueseDurationExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new PortugueseDatePeriodExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new PortugueseTimePeriodExtractorConfiguration());
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new PortugueseDateTimePeriodExtractorConfiguration());
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
