using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.English.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Resources.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateExtractorConfiguration : IDateExtractorConfiguration
    {
        public static readonly Regex MonthRegex =
            new Regex(
                DateTimeDefinition.MonthRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(
                DateTimeDefinition.DayRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(DateTimeDefinition.MonthNumRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegex = new Regex(DateTimeDefinition.YearRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayRegex =
            new Regex(
                DateTimeDefinition.WeekDayRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex OnRegex = new Regex(DateTimeDefinition.OnRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelaxedOnRegex =
            new Regex(
                DateTimeDefinition.RelaxedOnRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ThisRegex = new Regex(DateTimeDefinition.ThisRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LastRegex = new Regex(DateTimeDefinition.LastRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextRegex = new Regex(DateTimeDefinition.NextRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex = new Regex(DateTimeDefinition.UnitRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecialDayRegex =
            new Regex(
                DateTimeDefinition.SpecialDayRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex StrictWeekDay =
            new Regex(
                DateTimeDefinition.StrictWeekDay,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayOfMonthRegex =
            new Regex(
                DateTimeDefinition.WeekDayOfMonthRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecialDate = new Regex(DateTimeDefinition.SpecialDate,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] DateRegexList =
        {
            // (Sunday,)? April 5
            new Regex(
                DateTimeDefinition.DateExtractor1,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (Sunday,)? April 5, 2016
            new Regex(
                DateTimeDefinition.DateExtractor2, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (Sunday,)? 6th of April
            new Regex(
                DateTimeDefinition.DateExtractor3, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 3-23-2017
            new Regex(DateTimeDefinition.DateExtractor4,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 23-3-2015
            new Regex(DateTimeDefinition.DateExtractor5,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // on 1.3
            new Regex(DateTimeDefinition.DateExtractor6,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 7/23
            new Regex(DateTimeDefinition.DateExtractor7,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // on 24-12
            new Regex(DateTimeDefinition.DateExtractor8,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 23/7
            new Regex(DateTimeDefinition.DateExtractor9,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 2015-12-23
            new Regex(DateTimeDefinition.DateExtractorA,
                RegexOptions.IgnoreCase | RegexOptions.Singleline)
        };


        public static readonly Regex[] ImplicitDateList =
        {
            OnRegex, RelaxedOnRegex, SpecialDayRegex, ThisRegex, LastRegex, NextRegex,
            StrictWeekDay, WeekDayOfMonthRegex, SpecialDate
        };

        public static readonly Regex OfMonth = new Regex(DateTimeDefinition.OfMonth,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthEnd = new Regex(DateTimeDefinition.MonthEnd,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NonDateUnitRegex = new Regex(DateTimeDefinition.NonDateUnitRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public EnglishDateExtractorConfiguration()
        {
            IntegerExtractor = new IntegerExtractor();
            OrdinalExtractor = new OrdinalExtractor();
            NumberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
            UtilityConfiguration = new EnlighDatetimeUtilityConfiguration();
        }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IParser NumberParser { get; }

        public IExtractor DurationExtractor { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        IEnumerable<Regex> IDateExtractorConfiguration.DateRegexList => DateRegexList;

        IEnumerable<Regex> IDateExtractorConfiguration.ImplicitDateList => ImplicitDateList;

        Regex IDateExtractorConfiguration.OfMonth => OfMonth;

        Regex IDateExtractorConfiguration.MonthEnd => MonthEnd;

        Regex IDateExtractorConfiguration.NonDateUnitRegex => NonDateUnitRegex;
    }
}