﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.DateTime.German.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.German;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanCommonDateTimeParserConfiguration : BaseDateParserConfiguration
    {
        public GermanCommonDateTimeParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            UtilityConfiguration = new GermanDatetimeUtilityConfiguration();

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

            CardinalExtractor = Number.German.CardinalExtractor.GetInstance(numConfig);
            IntegerExtractor = Number.German.IntegerExtractor.GetInstance(numConfig);
            OrdinalExtractor = Number.German.OrdinalExtractor.GetInstance(numConfig);

            NumberParser = new BaseNumberParser(new GermanNumberParserConfiguration(numConfig));

            DateExtractor = new BaseDateExtractor(new GermanDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new GermanTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new GermanDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new GermanDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new GermanTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new GermanDateTimePeriodExtractorConfiguration(this));
            DurationParser = new BaseDurationParser(new GermanDurationParserConfiguration(this));
            DateParser = new BaseDateParser(new GermanDateParserConfiguration(this));
            TimeParser = new TimeParser(new GermanTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new GermanDateTimeParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new GermanDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new GermanTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new GermanDateTimePeriodParserConfiguration(this));
        }

        public override IImmutableDictionary<string, int> DayOfMonth => BaseDateTime.DayOfMonthDictionary.ToImmutableDictionary().AddRange(DateTimeDefinitions.DayOfMonth);
    }
}