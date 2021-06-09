using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.DateTime.Korean;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Korean;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanCommonDateTimeParserConfiguration : BaseCJKDateParserConfiguration, ICJKCommonDateTimeParserConfiguration
    {
        public KoreanCommonDateTimeParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToImmutableDictionary(k => k.Key, k => k.Value);
            UnitValueMap = DateTimeDefinitions.DurationUnitValueMap.ToImmutableDictionary();
            CardinalMap = DateTimeDefinitions.ParserConfigurationCardinalMap.ToImmutableDictionary();
            DayOfMonth = DateTimeDefinitions.ParserConfigurationDayOfMonth.ToImmutableDictionary();
            DayOfWeek = DateTimeDefinitions.ParserConfigurationDayOfWeek.ToImmutableDictionary();
            MonthOfYear = DateTimeDefinitions.ParserConfigurationMonthOfYear.ToImmutableDictionary();

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            IntegerExtractor = new IntegerExtractor(numConfig);
            CardinalExtractor = new CardinalExtractor(numConfig);
            OrdinalExtractor = new OrdinalExtractor(numConfig);

            NumberParser = new BaseCJKNumberParser(new KoreanNumberParserConfiguration(numConfig));

            // Do not change order. The order of initialization can lead to side-effects
            DateExtractor = new BaseCJKDateExtractor(new KoreanDateExtractorConfiguration(this));
            TimeExtractor = new BaseCJKTimeExtractor(new KoreanTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseCJKDateTimeExtractor(new KoreanDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseCJKDurationExtractor(new KoreanDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseCJKDatePeriodExtractor(new KoreanDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseCJKTimePeriodExtractor(new KoreanTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseCJKDateTimePeriodExtractor(new KoreanDateTimePeriodExtractorConfiguration(this));
            HolidayExtractor = new BaseCJKDurationExtractor(new KoreanDurationExtractorConfiguration(this));
            SetExtractor = new BaseCJKDurationExtractor(new KoreanDurationExtractorConfiguration(this));

            DurationParser = new BaseCJKDurationParser(new KoreanDurationParserConfiguration(this));
            DateParser = new BaseCJKDateParser(new KoreanDateParserConfiguration(this));
            TimeParser = new BaseCJKTimeParser(new KoreanTimeParserConfiguration(this));
            DateTimeParser = new BaseCJKDateTimeParser(new KoreanDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseCJKDatePeriodParser(new KoreanDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseCJKTimePeriodParser(new KoreanTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseCJKDateTimePeriodParser(new KoreanDateTimePeriodParserConfiguration(this));
            HolidayParser = new BaseCJKHolidayParser(new KoreanHolidayParserConfiguration(this));
            SetParser = new BaseCJKSetParser(new KoreanSetParserConfiguration(this));
        }
    }
}