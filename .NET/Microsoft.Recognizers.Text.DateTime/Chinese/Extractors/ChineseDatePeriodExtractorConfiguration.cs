﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDatePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKDatePeriodExtractorConfiguration
    {
        public static readonly Regex TillRegex = new Regex(DateTimeDefinitions.DatePeriodTillRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex RangePrefixRegex = new Regex(DateTimeDefinitions.DatePeriodRangePrefixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex RangeSuffixRegex = new Regex(DateTimeDefinitions.DatePeriodRangeSuffixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex StrictYearRegex = new Regex(DateTimeDefinitions.StrictYearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex YearInCJKRegex = new Regex(DateTimeDefinitions.DatePeriodYearInCJKRegex, RegexFlags, RegexTimeOut);

        // for case "(从)?(2017年)?一月十日到十二日"
        public static readonly Regex SimpleCasesRegex = new Regex(DateTimeDefinitions.SimpleCasesRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex YearAndMonth = new Regex(DateTimeDefinitions.YearAndMonth, RegexFlags, RegexTimeOut);

        // 2017.12, 2017-12, 2017/12, 12/2017
        public static readonly Regex PureNumYearAndMonth = new Regex(DateTimeDefinitions.PureNumYearAndMonth, RegexFlags, RegexTimeOut);

        public static readonly Regex SimpleYearAndMonth = new Regex(DateTimeDefinitions.SimpleYearAndMonth, RegexFlags, RegexTimeOut);

        public static readonly Regex OneWordPeriodRegex = new Regex(DateTimeDefinitions.OneWordPeriodRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WeekOfMonthRegex = new Regex(DateTimeDefinitions.WeekOfMonthRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WeekOfYearRegex = new Regex(DateTimeDefinitions.WeekOfYearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WeekOfDateRegex = new Regex(DateTimeDefinitions.WeekOfDateRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthOfDateRegex = new Regex(DateTimeDefinitions.MonthOfDateRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WhichWeekRegex = new Regex(DateTimeDefinitions.WhichWeekRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FollowedUnit = new Regex(DateTimeDefinitions.FollowedUnit, RegexFlags, RegexTimeOut);

        public static readonly Regex NumberCombinedWithUnit = new Regex(DateTimeDefinitions.NumberCombinedWithUnit, RegexFlags, RegexTimeOut);

        public static readonly Regex YearToYear = new Regex(DateTimeDefinitions.YearToYear, RegexFlags, RegexTimeOut);

        public static readonly Regex YearToYearSuffixRequired = new Regex(DateTimeDefinitions.YearToYearSuffixRequired, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthToMonth = new Regex(DateTimeDefinitions.MonthToMonth, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthToMonthSuffixRequired = new Regex(DateTimeDefinitions.MonthToMonthSuffixRequired, RegexFlags, RegexTimeOut);

        public static readonly Regex DayToDay = new Regex(DateTimeDefinitions.DayToDay, RegexFlags, RegexTimeOut);

        public static readonly Regex MonthDayRange = new Regex(DateTimeDefinitions.MonthDayRange, RegexFlags, RegexTimeOut);

        public static readonly Regex DayRegexForPeriod = new Regex(DateTimeDefinitions.DayRegexForPeriod, RegexFlags, RegexTimeOut);

        public static readonly Regex PastRegex = new Regex(DateTimeDefinitions.PastRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FutureRegex = new Regex(DateTimeDefinitions.FutureRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WeekWithWeekDayRangeRegex = new Regex(DateTimeDefinitions.WeekWithWeekDayRangeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FirstLastOfYearRegex = new Regex(DateTimeDefinitions.FirstLastOfYearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SeasonWithYear = new Regex(DateTimeDefinitions.SeasonWithYear, RegexFlags, RegexTimeOut);

        public static readonly Regex QuarterRegex = new Regex(DateTimeDefinitions.QuarterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DecadeRegex = new Regex(DateTimeDefinitions.DecadeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex CenturyRegex = new Regex(DateTimeDefinitions.CenturyRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex ComplexDatePeriodRegex = new Regex(DateTimeDefinitions.ComplexDatePeriodRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SpecialMonthRegex = new Regex(DateTimeDefinitions.SpecialMonthRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SpecialYearRegex = new Regex(DateTimeDefinitions.SpecialYearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DayRegex = new Regex(DateTimeDefinitions.DayRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex DayRegexInCJK = new Regex(DateTimeDefinitions.DatePeriodDayRegexInCJK, RegexFlags, RegexTimeOut);
        public static readonly Regex MonthNumRegex = new Regex(DateTimeDefinitions.MonthNumRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex ThisRegex = new Regex(DateTimeDefinitions.DatePeriodThisRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex DateUnitRegex = new Regex(DateTimeDefinitions.DateUnitRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.DatePeriodLastRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex NextRegex = new Regex(DateTimeDefinitions.DatePeriodNextRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex RelativeMonthRegex = new Regex(DateTimeDefinitions.RelativeMonthRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex LaterEarlyPeriodRegex = new Regex(DateTimeDefinitions.LaterEarlyPeriodRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex DatePointWithAgoAndLater = new Regex(DateTimeDefinitions.DatePointWithAgoAndLater, RegexFlags, RegexTimeOut);
        public static readonly Regex ReferenceDatePeriodRegex = new Regex(DateTimeDefinitions.ReferenceDatePeriodRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex MonthRegex = new Regex(DateTimeDefinitions.MonthRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex YearRegex = new Regex(DateTimeDefinitions.YearRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex YearRegexInNumber = new Regex(DateTimeDefinitions.YearRegexInNumber, RegexFlags, RegexTimeOut);
        public static readonly Regex ZeroToNineIntegerRegexCJK = new Regex(DateTimeDefinitions.ZeroToNineIntegerRegexCJK, RegexFlags, RegexTimeOut);
        public static readonly Regex MonthSuffixRegex = new Regex(DateTimeDefinitions.MonthSuffixRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.UnitRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex DurationUnitRegex = new Regex(DateTimeDefinitions.DurationUnitRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex SeasonRegex = new Regex(DateTimeDefinitions.SeasonRegex, RegexFlags, RegexTimeOut);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex[] SimpleCasesRegexes =
        {
            SimpleCasesRegex,
            OneWordPeriodRegex,
            StrictYearRegex,
            YearToYear,
            YearToYearSuffixRequired,
            MonthToMonth,
            MonthToMonthSuffixRequired,
            YearAndMonth,
            PureNumYearAndMonth,
            YearInCJKRegex,
            SpecialMonthRegex,
            SpecialYearRegex,
            WeekOfMonthRegex,
            WeekOfYearRegex,
            WeekOfDateRegex,
            MonthOfDateRegex,
            WhichWeekRegex,
            SeasonWithYear,
            QuarterRegex,
            DecadeRegex,
            CenturyRegex,
            ComplexDatePeriodRegex,
        };

        public ChineseDatePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DatePointExtractor = new BaseCJKDateExtractor(new ChineseDateExtractorConfiguration(this));
            DurationExtractor = new BaseCJKDurationExtractor(new ChineseDurationExtractorConfiguration(this));

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            IntegerExtractor = new IntegerExtractor(numConfig);
        }

        public IDateTimeExtractor DatePointExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        IEnumerable<Regex> ICJKDatePeriodExtractorConfiguration.SimpleCasesRegexes => SimpleCasesRegexes;

        Regex ICJKDatePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ICJKDatePeriodExtractorConfiguration.FutureRegex => FutureRegex;

        Regex ICJKDatePeriodExtractorConfiguration.PastRegex => PastRegex;

        Regex ICJKDatePeriodExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex ICJKDatePeriodExtractorConfiguration.FirstLastOfYearRegex => FirstLastOfYearRegex;

        Regex ICJKDatePeriodExtractorConfiguration.UnitRegex => UnitRegex;

        Regex ICJKDatePeriodExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

        Regex ICJKDatePeriodExtractorConfiguration.FollowedUnit => FollowedUnit;

        Regex ICJKDatePeriodExtractorConfiguration.RangePrefixRegex => RangePrefixRegex;

        Regex ICJKDatePeriodExtractorConfiguration.RangeSuffixRegex => RangeSuffixRegex;

        public Dictionary<Regex, Regex> AmbiguityFiltersDict => null;
    }
}