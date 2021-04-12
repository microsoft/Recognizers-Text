using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Arabic;
using Microsoft.Recognizers.Text.DateTime.Arabic.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Arabic;

namespace Microsoft.Recognizers.Text.DateTime.Arabic
{
    public class ArabicCommonDateTimeParserConfiguration : BaseDateParserConfiguration, ICommonDateTimeParserConfiguration
    {
        public ArabicCommonDateTimeParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            UtilityConfiguration = new ArabicDatetimeUtilityConfiguration();

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

            CardinalExtractor = Number.Arabic.CardinalExtractor.GetInstance(numConfig);
            IntegerExtractor = Number.Arabic.IntegerExtractor.GetInstance(numConfig);
            OrdinalExtractor = Number.Arabic.OrdinalExtractor.GetInstance(numConfig);

            NumberParser = new BaseNumberParser(new ArabicNumberParserConfiguration(numConfig));

            TimeZoneParser = new BaseTimeZoneParser();

            // Do not change order. The order of initialization can lead to side-effects
            DateExtractor = new BaseDateExtractor(new ArabicDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new ArabicTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new ArabicDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new ArabicDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new ArabicDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new ArabicTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new ArabicDateTimePeriodExtractorConfiguration(this));

            DurationParser = new BaseDurationParser(new ArabicDurationParserConfiguration(this));
            DateParser = new BaseDateParser(new ArabicDateParserConfiguration(this));
            TimeParser = new TimeParser(new ArabicTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new ArabicDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new ArabicDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new ArabicTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new ArabicDateTimePeriodParserConfiguration(this));

            DateTimeAltParser = new BaseDateTimeAltParser(new ArabicDateTimeAltParserConfiguration(this));
        }

        public override IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary().AddRange(DateTimeDefinitions.DayOfMonth);
    }
}