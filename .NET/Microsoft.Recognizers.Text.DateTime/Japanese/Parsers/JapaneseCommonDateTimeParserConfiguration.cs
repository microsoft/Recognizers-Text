using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.DateTime.Japanese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseCommonDateTimeParserConfiguration : BaseCJKDateParserConfiguration, ICJKCommonDateTimeParserConfiguration
    {
        public JapaneseCommonDateTimeParserConfiguration(IDateTimeOptionsConfiguration config)
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

            NumberParser = new BaseCJKNumberParser(new JapaneseNumberParserConfiguration(numConfig));

            // Do not change order. The order of initialization can lead to side-effects
            DateExtractor = new BaseCJKDateExtractor(new JapaneseDateExtractorConfiguration(this));
            TimeExtractor = new BaseCJKTimeExtractor(new JapaneseTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseCJKDateTimeExtractor(new JapaneseDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseCJKDurationExtractor(new JapaneseDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseCJKDatePeriodExtractor(new JapaneseDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseCJKTimePeriodExtractor(new JapaneseTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseCJKDateTimePeriodExtractor(new JapaneseDateTimePeriodExtractorConfiguration(this));
            HolidayExtractor = new BaseCJKDurationExtractor(new JapaneseDurationExtractorConfiguration(this));
            SetExtractor = new BaseCJKDurationExtractor(new JapaneseDurationExtractorConfiguration(this));

            DurationParser = new BaseCJKDurationParser(new JapaneseDurationParserConfiguration(this));
            DateParser = new BaseCJKDateParser(new JapaneseDateParserConfiguration(this));
            TimeParser = new BaseCJKTimeParser(new JapaneseTimeParserConfiguration(this));
            DateTimeParser = new BaseCJKDateTimeParser(new JapaneseDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseCJKDatePeriodParser(new JapaneseDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseCJKTimePeriodParser(new JapaneseTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseCJKDateTimePeriodParser(new JapaneseDateTimePeriodParserConfiguration(this));
            HolidayParser = new BaseCJKHolidayParser(new JapaneseHolidayParserConfiguration(this));
            SetParser = new BaseCJKSetParser(new JapaneseSetParserConfiguration(this));
        }
    }
}