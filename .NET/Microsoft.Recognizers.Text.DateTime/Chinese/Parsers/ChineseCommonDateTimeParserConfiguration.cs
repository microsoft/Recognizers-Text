using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseCommonDateTimeParserConfiguration : BaseCJKDateParserConfiguration, ICJKCommonDateTimeParserConfiguration
    {
        public ChineseCommonDateTimeParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToImmutableDictionary(k => k.Key, k => k.Value.Substring(0, 1) + k.Value.Substring(1).ToLower());
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

            NumberParser = new BaseCJKNumberParser(new ChineseNumberParserConfiguration(numConfig));

            // Do not change order. The order of initialization can lead to side-effects
            DateExtractor = new BaseCJKDateExtractor(new ChineseDateExtractorConfiguration(this));
            TimeExtractor = new BaseCJKTimeExtractor(new ChineseTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseCJKDateTimeExtractor(new ChineseDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseCJKDurationExtractor(new ChineseDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseCJKDatePeriodExtractor(new ChineseDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseCJKTimePeriodExtractor(new ChineseTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseCJKDateTimePeriodExtractor(new ChineseDateTimePeriodExtractorConfiguration(this));
            HolidayExtractor = new BaseCJKDurationExtractor(new ChineseDurationExtractorConfiguration(this));
            SetExtractor = new BaseCJKDurationExtractor(new ChineseDurationExtractorConfiguration(this));

            DurationParser = new BaseCJKDurationParser(new ChineseDurationParserConfiguration(this));
            DateParser = new BaseCJKDateParser(new ChineseDateParserConfiguration(this));
            TimeParser = new BaseCJKTimeParser(new ChineseTimeParserConfiguration(this));
            DateTimeParser = new BaseCJKDateTimeParser(new ChineseDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseCJKDatePeriodParser(new ChineseDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseCJKTimePeriodParser(new ChineseTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseCJKDateTimePeriodParser(new ChineseDateTimePeriodParserConfiguration(this));
            HolidayParser = new BaseCJKHolidayParser(new ChineseHolidayParserConfiguration(this));
            SetParser = new BaseCJKSetParser(new ChineseSetParserConfiguration(this));
        }
    }
}