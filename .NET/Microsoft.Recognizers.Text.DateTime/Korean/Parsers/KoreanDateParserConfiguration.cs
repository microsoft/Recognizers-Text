﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanDateParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDateParserConfiguration
    {
        public static readonly Regex NextMonthRegex = new Regex(DateTimeDefinitions.ParserConfigurationNextMonthRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex LastMonthRegex = new Regex(DateTimeDefinitions.ParserConfigurationLastMonthRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex LastWeekDayRegex = new Regex(DateTimeDefinitions.ParserConfigurationLastWeekDayRegex, RegexFlags, RegexTimeOut);

        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; // "Date";

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public KoreanDateParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
             : base(config)
        {
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;

            NumberParser = config.NumberParser;

            DateExtractor = config.DateExtractor;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;

            DateRegexList = new KoreanDateExtractorConfiguration(this).DateRegexList;
            SpecialDate = KoreanDateExtractorConfiguration.SpecialDate;
            NextRe = KoreanDateExtractorConfiguration.NextRe;
            LastRe = KoreanDateExtractorConfiguration.LastRe;
            SpecialDayRegex = KoreanDateExtractorConfiguration.SpecialDayRegex;
            StrictWeekDayRegex = KoreanDateExtractorConfiguration.WeekDayRegex;
            LunarRegex = KoreanDateExtractorConfiguration.LunarRegex;
            UnitRegex = KoreanDateExtractorConfiguration.UnitRegex;
            BeforeRegex = KoreanDateExtractorConfiguration.BeforeRegex;
            AfterRegex = KoreanDateExtractorConfiguration.AfterRegex;
            DynastyYearRegex = KoreanDateExtractorConfiguration.DynastyYearRegex;
            DynastyStartYear = KoreanDateExtractorConfiguration.DynastyStartYear;
            DynastyYearMap = KoreanDateExtractorConfiguration.DynastyYearMap;
            NextRegex = KoreanDateExtractorConfiguration.NextRegex;
            ThisRegex = KoreanDateExtractorConfiguration.ThisRegex;
            LastRegex = KoreanDateExtractorConfiguration.LastRegex;
            WeekDayOfMonthRegex = KoreanDateExtractorConfiguration.WeekDayOfMonthRegex;
            WeekDayAndDayRegex = KoreanDateExtractorConfiguration.WeekDayAndDayRegex;
            DurationRelativeDurationUnitRegex = KoreanDateExtractorConfiguration.DurationRelativeDurationUnitRegex;
            SpecialDayWithNumRegex = KoreanDateExtractorConfiguration.SpecialDayWithNumRegex;

            CardinalMap = config.CardinalMap;
            UnitMap = config.UnitMap;
            DayOfMonth = config.DayOfMonth;
            DayOfWeek = config.DayOfWeek;
            MonthOfYear = config.MonthOfYear;

        }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IEnumerable<Regex> DateRegexList { get; }

        public Regex SpecialDate { get; }

        public Regex NextRe { get; }

        public Regex LastRe { get; }

        public Regex SpecialDayRegex { get; }

        public Regex StrictWeekDayRegex { get; }

        public Regex LunarRegex { get; }

        public Regex UnitRegex { get; }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex NextRegex { get; }

        public Regex ThisRegex { get; }

        public Regex LastRegex { get; }

        public Regex WeekDayOfMonthRegex { get; }

        public Regex WeekDayAndDayRegex { get; }

        public Regex DurationRelativeDurationUnitRegex { get; }

        public Regex SpecialDayWithNumRegex { get; }

        public Regex DynastyYearRegex { get; }

        public ImmutableDictionary<string, int> DynastyYearMap { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> DayOfWeek { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public string DynastyStartYear { get; }

        Regex ICJKDateParserConfiguration.LastWeekDayRegex => LastWeekDayRegex;

        Regex ICJKDateParserConfiguration.NextMonthRegex => NextMonthRegex;

        Regex ICJKDateParserConfiguration.LastMonthRegex => LastMonthRegex;

        public int GetSwiftDay(string text)
        {
            var value = 0;

            // @TODO move hardcoded values to resources file
            if (text.Equals("今天", StringComparison.Ordinal) ||
                text.Equals("今日", StringComparison.Ordinal) ||
                text.Equals("最近", StringComparison.Ordinal))
            {
                value = 0;
            }
            else if (text.StartsWith("明", StringComparison.Ordinal))
            {
                value = 1;
            }
            else if (text.StartsWith("昨", StringComparison.Ordinal))
            {
                value = -1;
            }
            else if (text.Equals("大后天", StringComparison.Ordinal) ||
                     text.Equals("大後天", StringComparison.Ordinal))
            {
                value = 3;
            }
            else if (text.Equals("大前天", StringComparison.Ordinal))
            {
                value = -3;
            }
            else if (text.Equals("后天", StringComparison.Ordinal) ||
                     text.Equals("後天", StringComparison.Ordinal))
            {
                value = 2;
            }
            else if (text.Equals("前天", StringComparison.Ordinal))
            {
                value = -2;
            }

            return value;
        }
    }
}
