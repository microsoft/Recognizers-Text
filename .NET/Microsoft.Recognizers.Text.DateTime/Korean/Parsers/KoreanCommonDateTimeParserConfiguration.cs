using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.DateTime.Korean.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Korean;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanCommonDateTimeParserConfiguration : BaseDateParserConfiguration, ICommonDateTimeParserConfiguration
    {
        public KoreanCommonDateTimeParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            UtilityConfiguration = new KoreanDatetimeUtilityConfiguration();

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
            KoreanNumberExtractorMode mode = KoreanNumberExtractorMode.Default;

            CardinalExtractor = new CardinalExtractor(mode);
            IntegerExtractor = new IntegerExtractor(mode);
            OrdinalExtractor = new OrdinalExtractor();

            NumberParser = new BaseCJKNumberParser(new KoreanNumberParserConfiguration(numConfig));

            TimeZoneParser = new BaseTimeZoneParser();

            // Do not change order. The order of initialization can lead to side-effects
            DateExtractor = new BaseDateExtractor(new KoreanDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new KoreanTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new KoreanDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new KoreanDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new KoreanDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new KoreanTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new KoreanDateTimePeriodExtractorConfiguration(this));

            DurationParser = new BaseDurationParser(new KoreanDurationParserConfiguration(this));
            DateParser = new BaseDateParser(new KoreanDateParserConfiguration(this));
            TimeParser = new TimeParser(new KoreanTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new KoreanDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new KoreanDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new KoreanTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new KoreanDateTimePeriodParserConfiguration(this));

            DateTimeAltParser = new BaseDateTimeAltParser(new KoreanDateTimeAltParserConfiguration(this));
        }

        public override IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary().AddRange(DateTimeDefinitions.DayOfMonth);
    }
}