// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Swedish;
using Microsoft.Recognizers.Text.DateTime.Swedish.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Swedish;

namespace Microsoft.Recognizers.Text.DateTime.Swedish
{
    public class SwedishCommonDateTimeParserConfiguration : BaseDateParserConfiguration, ICommonDateTimeParserConfiguration
    {
        public SwedishCommonDateTimeParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            UtilityConfiguration = new SwedishDatetimeUtilityConfiguration();

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

            CardinalExtractor = Number.Swedish.CardinalExtractor.GetInstance(numConfig);
            IntegerExtractor = Number.Swedish.IntegerExtractor.GetInstance(numConfig);
            OrdinalExtractor = Number.Swedish.OrdinalExtractor.GetInstance(numConfig);

            NumberParser = new BaseNumberParser(new SwedishNumberParserConfiguration(numConfig));

            TimeZoneParser = new BaseTimeZoneParser();

            // Do not change order. The order of initialization can lead to side-effects
            DateExtractor = new BaseDateExtractor(new SwedishDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new SwedishTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new SwedishDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new SwedishDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new SwedishDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new SwedishTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new SwedishDateTimePeriodExtractorConfiguration(this));

            DurationParser = new BaseDurationParser(new SwedishDurationParserConfiguration(this));
            DateParser = new BaseDateParser(new SwedishDateParserConfiguration(this));
            TimeParser = new TimeParser(new SwedishTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new SwedishDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new SwedishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new SwedishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new SwedishDateTimePeriodParserConfiguration(this));

            DateTimeAltParser = new BaseDateTimeAltParser(new SwedishDateTimeAltParserConfiguration(this));
        }

        public override IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary().AddRange(DateTimeDefinitions.DayOfMonth);
    }
}