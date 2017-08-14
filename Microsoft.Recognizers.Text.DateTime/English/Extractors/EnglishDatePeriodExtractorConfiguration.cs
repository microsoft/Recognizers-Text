using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Resources.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDatePeriodExtractorConfiguration : IDatePeriodExtractorConfiguration
    {
        // base regexes
        public static readonly Regex TillRegex = new Regex(DateTimeDefinitions.TillRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AndRegex = new Regex(DateTimeDefinitions.AndRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(
                DateTimeDefinitions.DayRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(DateTimeDefinitions.MonthNumRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegex = new Regex(DateTimeDefinitions.YearRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayRegex =
            new Regex(
                DateTimeDefinitions.WeekDayRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeMonthRegex = new Regex(DateTimeDefinitions.RelativeMonthRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EngMonthRegex =
            new Regex(
                DateTimeDefinitions.EngMonthRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthSuffixRegex =
            new Regex(DateTimeDefinitions.MonthSuffixRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.UnitRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastRegex = new Regex(DateTimeDefinitions.PastRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FutureRegex = new Regex(DateTimeDefinitions.FutureRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // composite regexes
        public static readonly Regex SimpleCasesRegex =
            new Regex(
                DateTimeDefinitions.SimpleCasesRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthFrontSimpleCasesRegex =
            new Regex(
                DateTimeDefinitions.MonthFrontSimpleCasesRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthFrontBetweenRegex =
            new Regex(
                DateTimeDefinitions.MonthFrontBetweenRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BetweenRegex =
            new Regex(
                DateTimeDefinitions.BetweenRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthWithYear =
            new Regex(
                DateTimeDefinitions.MonthWithYear,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex OneWordPeriodRegex =
            new Regex(
                DateTimeDefinitions.OneWordPeriodRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumWithYear =
            new Regex(DateTimeDefinitions.MonthNumWithYear,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);


        public static readonly Regex WeekOfMonthRegex =
            new Regex(
                DateTimeDefinitions.WeekOfMonthRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekOfYearRegex =
            new Regex(
                DateTimeDefinitions.WeekOfYearRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedUnit = new Regex(DateTimeDefinitions.FollowedUnit,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithUnit =
            new Regex(DateTimeDefinitions.NumberCombinedWithUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegex =
            new Regex(
                DateTimeDefinitions.QuarterRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegexYearFront =
            new Regex(
                DateTimeDefinitions.QuarterRegexYearFront,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SeasonRegex =
            new Regex(
                DateTimeDefinitions.SeasonRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WhichWeekRegex =
            new Regex(
                DateTimeDefinitions.WhichWeekRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekOfRegex =
            new Regex(
                DateTimeDefinitions.WeekOfRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthOfRegex =
            new Regex(
                DateTimeDefinitions.MonthOfRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);


        private static readonly Regex[] SimpleCasesRegexes =
        {
            SimpleCasesRegex,
            BetweenRegex,
            OneWordPeriodRegex,
            MonthWithYear,
            MonthNumWithYear,
            YearRegex,
            WeekOfMonthRegex,
            WeekOfYearRegex,
            MonthFrontBetweenRegex,
            MonthFrontSimpleCasesRegex,
            QuarterRegex,
            QuarterRegexYearFront,
            SeasonRegex,
            WhichWeekRegex
        };

        public EnglishDatePeriodExtractorConfiguration()
        {
            DatePointExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
            CardinalExtractor = new CardinalExtractor();
        }

        public IExtractor DatePointExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        IEnumerable<Regex> IDatePeriodExtractorConfiguration.SimpleCasesRegexes => SimpleCasesRegexes;

        Regex IDatePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex IDatePeriodExtractorConfiguration.FollowedUnit => FollowedUnit;

        Regex IDatePeriodExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

        Regex IDatePeriodExtractorConfiguration.PastRegex => PastRegex;

        Regex IDatePeriodExtractorConfiguration.FutureRegex => FutureRegex;

        Regex IDatePeriodExtractorConfiguration.WeekOfRegex => WeekOfRegex;

        Regex IDatePeriodExtractorConfiguration.MonthOfRegex => MonthOfRegex;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("from"))
            {
                index = text.LastIndexOf("from", StringComparison.Ordinal);
                return true;
            }
            return false;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("between"))
            {
                index = text.LastIndexOf("between", StringComparison.Ordinal);
                return true;
            }
            return false;
        }

        public bool HasConnectorToken(string text)
        {
            return text.Equals("and");
        }
    }
}