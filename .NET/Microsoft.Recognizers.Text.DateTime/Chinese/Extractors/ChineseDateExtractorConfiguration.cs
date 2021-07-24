﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Chinese;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDateExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKDateExtractorConfiguration
    {

        public static readonly Regex WeekDayRegex = new Regex(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        public static readonly Regex LunarRegex = new Regex(DateTimeDefinitions.LunarRegex, RegexFlags);

        public static readonly Regex ThisRegex = new Regex(DateTimeDefinitions.DateThisRegex, RegexFlags);

        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.DateLastRegex, RegexFlags);

        public static readonly Regex NextRegex = new Regex(DateTimeDefinitions.DateNextRegex, RegexFlags);

        public static readonly Regex SpecialDayRegex = new Regex(DateTimeDefinitions.SpecialDayRegex, RegexFlags);

        public static readonly Regex WeekDayOfMonthRegex = new Regex(DateTimeDefinitions.WeekDayOfMonthRegex, RegexFlags);

        public static readonly Regex SpecialDate = new Regex(DateTimeDefinitions.SpecialDate, RegexFlags);

        public static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex AfterRegex = new Regex(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex DateTimePeriodUnitRegex = new Regex(DateTimeDefinitions.DateTimePeriodUnitRegex, RegexFlags);

        public static readonly Regex MonthRegex = new Regex(DateTimeDefinitions.MonthRegex, RegexFlags);
        public static readonly Regex DayRegex = new Regex(DateTimeDefinitions.DayRegex, RegexFlags);
        public static readonly Regex DayRegexInCJK = new Regex(DateTimeDefinitions.DateDayRegexInCJK, RegexFlags);
        public static readonly Regex DayRegexNumInCJK = new Regex(DateTimeDefinitions.DayRegexNumInCJK, RegexFlags);
        public static readonly Regex MonthNumRegex = new Regex(DateTimeDefinitions.MonthNumRegex, RegexFlags);
        public static readonly Regex YearRegex = new Regex(DateTimeDefinitions.YearRegex, RegexFlags);
        public static readonly Regex RelativeRegex = new Regex(DateTimeDefinitions.RelativeRegex, RegexFlags);
        public static readonly Regex ZeroToNineIntegerRegexCJK = new Regex(DateTimeDefinitions.ZeroToNineIntegerRegexCJK, RegexFlags);
        public static readonly Regex YearInCJKRegex = new Regex(DateTimeDefinitions.DateYearInCJKRegex, RegexFlags);
        public static readonly Regex ThisRe = new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexFlags);
        public static readonly Regex LastRe = new Regex(DateTimeDefinitions.LastPrefixRegex, RegexFlags);
        public static readonly Regex NextRe = new Regex(DateTimeDefinitions.NextPrefixRegex, RegexFlags);
        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.DateUnitRegex, RegexFlags);
        public static readonly Regex DynastyYearRegex = new Regex(DateTimeDefinitions.DynastyYearRegex, RegexFlags);
        public static readonly string DynastyStartYear = DateTimeDefinitions.DynastyStartYear;
        public static readonly ImmutableDictionary<string, int> DynastyYearMap = DateTimeDefinitions.DynastyYearMap.ToImmutableDictionary();

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ChineseDateExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            var durationConfig = new BaseDateTimeOptionsConfiguration(config.Culture, DateTimeOptions.None);

            DurationExtractor = new BaseCJKDurationExtractor(new ChineseDurationExtractorConfiguration(durationConfig));

            ImplicitDateList = new List<Regex>
            {
                LunarRegex, SpecialDayRegex, ThisRegex, LastRegex, NextRegex,
                WeekDayRegex, WeekDayOfMonthRegex, SpecialDate,
            };

            // (农历)?(2016年)?一月三日(星期三)?
            var dateRegex1 = new Regex(DateTimeDefinitions.DateRegexList1, RegexFlags);

            // (2015年)?(农历)?十月初一(星期三)?
            var dateRegex2 = new Regex(DateTimeDefinitions.DateRegexList2, RegexFlags);

            // (2015年)?(农历)?十月二十(星期三)?
            var dateRegex3 = new Regex(DateTimeDefinitions.DateRegexList3, RegexFlags);

            // 2015-12-23 - This regex represents the standard format in Chinese dates (YMD) and has precedence over other orderings
            var dateRegex8 = new Regex(DateTimeDefinitions.DateRegexList8, RegexFlags);

            // 23/7
            var dateRegex5 = new Regex(DateTimeDefinitions.DateRegexList5, RegexFlags);

            // 7/23
            var dateRegex4 = new Regex(DateTimeDefinitions.DateRegexList4, RegexFlags);

            // 23-3-2017
            var dateRegex7 = new Regex(DateTimeDefinitions.DateRegexList7, RegexFlags);

            // 3-23-2015
            var dateRegex6 = new Regex(DateTimeDefinitions.DateRegexList6, RegexFlags);

            // Regex precedence where the order between D and M varies is controlled by DefaultLanguageFallback
            var enableDmy = DateTimeDefinitions.DefaultLanguageFallback == Constants.DefaultLanguageFallback_DMY;
            var enableYmd = DateTimeDefinitions.DefaultLanguageFallback == Constants.DefaultLanguageFallback_YMD;

            DateRegexList = new List<Regex> { dateRegex1, dateRegex2, dateRegex3, dateRegex8 };
            DateRegexList = DateRegexList.Concat(
                enableDmy ?
                new[] { dateRegex5, dateRegex4, dateRegex7, dateRegex6 } :
                enableYmd ?
                new[] { dateRegex4, dateRegex5, dateRegex7, dateRegex6 } :
                new[] { dateRegex4, dateRegex5, dateRegex6, dateRegex7 });

        }

        public IEnumerable<Regex> DateRegexList { get; }

        public IEnumerable<Regex> ImplicitDateList { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        Regex ICJKDateExtractorConfiguration.DateTimePeriodUnitRegex => DateTimePeriodUnitRegex;

        Regex ICJKDateExtractorConfiguration.BeforeRegex => BeforeRegex;

        Regex ICJKDateExtractorConfiguration.AfterRegex => AfterRegex;

    }
}