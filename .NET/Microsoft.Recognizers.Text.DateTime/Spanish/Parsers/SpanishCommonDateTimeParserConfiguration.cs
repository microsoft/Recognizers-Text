using System.Collections.Immutable;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.DateTime.Spanish.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Spanish;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishCommonDateTimeParserConfiguration : BaseDateParserConfiguration
    {
        public SpanishCommonDateTimeParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            UtilityConfiguration = new SpanishDatetimeUtilityConfiguration();

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

            CardinalExtractor = Number.Spanish.CardinalExtractor.GetInstance(numConfig);
            IntegerExtractor = Number.Spanish.IntegerExtractor.GetInstance(numConfig);
            OrdinalExtractor = Number.Spanish.OrdinalExtractor.GetInstance(numConfig);

            NumberParser = new BaseNumberParser(new SpanishNumberParserConfiguration(numConfig));

            DateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration(this));
            DateParser = new BaseDateParser(new SpanishDateParserConfiguration(this));
            TimeParser = new BaseTimeParser(new SpanishTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new SpanishDateTimeParserConfiguration(this));
            DurationParser = new BaseDurationParser(new SpanishDurationParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new SpanishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new SpanishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new SpanishDateTimePeriodParserConfiguration(this));
            DateTimeAltParser = new BaseDateTimeAltParser(new SpanishDateTimeAltParserConfiguration(this));
        }
    }
}
