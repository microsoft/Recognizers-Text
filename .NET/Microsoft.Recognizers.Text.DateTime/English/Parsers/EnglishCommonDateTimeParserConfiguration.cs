using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.DateTime.English.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishCommonDateTimeParserConfiguration : BaseDateParserConfiguration, ICommonDateTimeParserConfiguration
    {
        public EnglishCommonDateTimeParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            UtilityConfiguration = new EnglishDatetimeUtilityConfiguration();

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

            CardinalExtractor = Number.English.CardinalExtractor.GetInstance(numConfig);
            IntegerExtractor = Number.English.IntegerExtractor.GetInstance(numConfig);
            OrdinalExtractor = Number.English.OrdinalExtractor.GetInstance(numConfig);

            NumberParser = new BaseNumberParser(new EnglishNumberParserConfiguration(numConfig));

            TimeZoneParser = new BaseTimeZoneParser();

            DateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration(this));
            DurationParser = new BaseDurationParser(new EnglishDurationParserConfiguration(this));
            DateParser = new BaseDateParser(new EnglishDateParserConfiguration(this));
            TimeParser = new TimeParser(new EnglishTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new EnglishDateTimePeriodParserConfiguration(this));
            DateTimeAltParser = new BaseDateTimeAltParser(new EnglishDateTimeAltParserConfiguration(this));
        }

        public override IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary().AddRange(DateTimeDefinitions.DayOfMonth);
    }
}